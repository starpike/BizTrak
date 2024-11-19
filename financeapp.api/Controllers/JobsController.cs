using System;
using FinanceApp.Data;
using FinanceApp.Domain;
using FinanceApp.DTO;
using FinanceApp.Services;
using FinanceApp.Services.Validation;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobsController(ILogger<QuotesController> logger, IUnitOfWork unitOfWork, IJobValidationService jobValidationService) : ControllerBase
{
    private readonly ILogger<QuotesController> logger = logger;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IJobValidationService jobValidationService = jobValidationService;

    [HttpGet]
    public async Task<IActionResult> ListJobs([FromQuery] DateTime start, DateTime end)
    {
        var jobs = await unitOfWork.Jobs.ListJobs(start, end);

        var dto = jobs.Adapt<IEnumerable<JobDTO>>();

        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> AddJob([FromBody] JobDTO jobDTO)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var validationResult = await jobValidationService.ValidateAsync(jobDTO);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var newJob = jobDTO.Adapt<Job>();

            await unitOfWork.Jobs.CreateJob(newJob);
            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("Quote created successfully.");
            return CreatedAtAction(nameof(AddJob), new { id = newJob.Id }, newJob);
        }
        catch (ValidationException ex)
        {
            logger.LogError(ex, "Validation error occurred while creating the job.");
            return BadRequest(ex.Errors);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "A database error occurred while creating the job.");
            return StatusCode(500, "A database error occurred. Please try again later.");
        }
    }
}
