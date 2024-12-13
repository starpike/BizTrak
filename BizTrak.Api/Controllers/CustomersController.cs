namespace BizTrak.Api;
using BizTrak.Data;
using BizTrak.Domain.Entities;
using BizTrak.Domain.Extensions;
using BizTrak.DTO;
using Microsoft.AspNetCore.Mvc;
using Mapster;
using Microsoft.EntityFrameworkCore;
using BizTrak.Application.Validation;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
public class CustomersController(ILogger<CustomersController> logger, ICustomerValidationsService customerValidationsService, IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly ILogger<CustomersController> logger = logger;
    private readonly ICustomerValidationsService customerValidationsService = customerValidationsService;
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    [HttpGet]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> ListCustomers([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = "")
    {
        var pagedCustomers = await unitOfWork.Customers.PagedCustomers(page, pageSize, search);
        var pagedCustomersDTO = pagedCustomers.Adapt<PagedCustomersDTO>();
        return Ok(pagedCustomersDTO);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await unitOfWork.Customers.GetCustomerAsync(id);

        if (customer == null)
            return NotFound();

        return Ok(customer);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerDTO customerDTO)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var validationResult = customerValidationsService.Validate(customerDTO);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var customer = await unitOfWork.Customers.GetCustomerAsync(id);
        if (customer == null) return NotFound();

        var updatedCustomer = customerDTO.Adapt<Customer>();
        unitOfWork.Customers.UpdateAsync(customer, updatedCustomer);

        await unitOfWork.SaveChangesAsync();

        logger.LogInformation("Customer updated successfully.");
        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerDTO customerDTO)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var validationResult = customerValidationsService.Validate(customerDTO);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);


        var customer = customerDTO.Adapt<Customer>();
        await unitOfWork.Customers.AddAsync(customer);
        await unitOfWork.SaveChangesAsync();

        logger.LogInformation("Customer created successfully.");
        return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customerDTO);
    }
}