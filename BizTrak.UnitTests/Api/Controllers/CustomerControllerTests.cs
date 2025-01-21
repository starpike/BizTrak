using System;
using BizTrak.Api;
using BizTrak.Application.Services;
using BizTrak.Application.Validation;
using BizTrak.Data;
using BizTrak.Domain.Entities;
using BizTrak.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BizTrak.UnitTests.Api.Controllers;

public class CustomerControllerTests
{
    private readonly Mock<ILogger<CustomersController>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICustomerValidationsService> _validationsServiceMock;
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly CustomersController _controller;
    private readonly IEnumerable<Customer> _testData;

    public CustomerControllerTests()
    {
        _loggerMock = new Mock<ILogger<CustomersController>>();
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _validationsServiceMock = new Mock<ICustomerValidationsService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _testData = [
            new Customer {
                    Id = 1,
                    Name = "Customer 1",
                    Address = "Address 1"
                }
        ];

        // Setup the IUnitOfWork to return the mock repository
        _unitOfWorkMock
            .Setup(uow => uow.Customers)
            .Returns(_customerRepositoryMock.Object);

        _controller = new CustomersController(
            _loggerMock.Object,
            _validationsServiceMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task GetCustomerById_WhenCustomerExists_ShouldReturnOkWithCustomer()
    {
        var customerId = 1;
        var customer = _testData.First(c => c.Id == customerId);

        _customerRepositoryMock
            .Setup(repo => repo.GetCustomerAsync(customerId))
            .ReturnsAsync(customer);

        // Act
        var result = await _controller.GetById(customerId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCustomer = Assert.IsType<Customer>(okResult.Value);
        Assert.Equal(customerId, returnedCustomer.Id);
    }

    [Fact]
    public async Task UpdateCustomer_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("Name", "Required");

        // Act
        var result = await _controller.UpdateCustomer(1, new CustomerDTO()
        {
            Id = 1,
            Name = "",
            Address = ""
        });

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task UpdateCustomer_ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange
        var customerDTO = new CustomerDTO() { Id = 1, Name = "", Address = "" };
        _validationsServiceMock.Setup(v => v.Validate(customerDTO)).Returns(new ValidationResult(false,
            [new ValidationError("", "Invalid customer data.")]));

        // Act
        var result = await _controller.UpdateCustomer(1, customerDTO);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task UpdateCustomer_ReturnsNotFound_WhenCustomerDoesNotExist()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.Customers.GetCustomerAsync(1)).ReturnsAsync((Customer)null!);
        _validationsServiceMock
            .Setup(v => v.Validate(It.IsAny<CustomerDTO>()))
            .Returns(new ValidationResult(true, []));

        // Act
        var result = await _controller.UpdateCustomer(1, It.IsAny<CustomerDTO>());

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task UpdateCustomer_ReturnsNoContent_WhenUpdateIsSuccessful()
    {
        // Arrange
        var customerDTO = new CustomerDTO { Id = 1, Name = "Updated Customer", Address = "" };
        var customer = new Customer { Id = 1, Name = "Old Customer", Address = "" };

        _unitOfWorkMock.Setup(u => u.Customers.GetCustomerAsync(1)).ReturnsAsync(customer);
        _validationsServiceMock.Setup(v => v.Validate(customerDTO)).Returns(new ValidationResult(true, []));
        _unitOfWorkMock.Setup(u => u.Customers.UpdateAsync(customer, It.IsAny<Customer>())).Verifiable();
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.FromResult(1));

        // Act
        var result = await _controller.UpdateCustomer(1, customerDTO);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _unitOfWorkMock.Verify(u => u.Customers.UpdateAsync(customer, It.IsAny<Customer>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Customer updated successfully.")),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }
}
