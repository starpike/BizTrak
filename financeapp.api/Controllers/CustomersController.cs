namespace FinanceApp.Api;
using FinanceApp.Data;
using FinanceApp.Domain;
using FinanceApp.Domain.Extensions;
using FinanceApp.DTO;
using Microsoft.AspNetCore.Mvc;
using Mapster;
using FinanceApp.Services;
using FinanceApp.Services.Validation;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class CustomersController(ILogger<CustomersController> logger, ICustomerValidationsService customerValidationsService, IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly ILogger<CustomersController> logger = logger;
    private readonly ICustomerValidationsService customerValidationsService = customerValidationsService;
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    [HttpGet]
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

        try
        {
            var customer = await unitOfWork.Customers.GetCustomerAsync(id);
            if (customer == null) return NotFound();

            var updatedCustomer = customerDTO.Adapt<Customer>();
            unitOfWork.Customers.UpdateAsync(customer, updatedCustomer);

            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("Customer updated successfully.");
            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            logger.LogError(ex, "A concurrency error occurred while updating the customer.");
            return Conflict("A concurrency error occurred while updating the customer.");
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "A database error occurred while updating the customer.");
            return StatusCode(500, "A database error occurred. Please try again later.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while updating the customer.");
            return StatusCode(500, "An unexpected error occurred. Please try again later.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerDTO customerDTO)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var validationResult = customerValidationsService.Validate(customerDTO);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        try
        {
            var customer = customerDTO.Adapt<Customer>();
            await unitOfWork.Customers.CreateAsync(customer);
            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("Customer created successfully.");
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customerDTO);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "A database error occurred while creating the customer.");
            return StatusCode(500, "A database error occurred. Please try again later.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating the customer.");
            return StatusCode(500, "Internal server error");
        }
    }
}