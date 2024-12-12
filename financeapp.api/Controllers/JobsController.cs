using System;
using FinanceApp.Api.Extensions;
using FinanceApp.Application.Validation;
using FinanceApp.Data;
using FinanceApp.Domain.Entities;
using FinanceApp.DTO;
using Mapster;
using Microsoft.AspNetCore.JsonPatch;
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

    [HttpGet("scheduled")]
    public async Task<IActionResult> Scheduled([FromQuery] DateTime start, DateTime end)
    {
        var jobs = await unitOfWork.Jobs.ListScheduledAsync(start, end);

        var dto = jobs.Adapt<IEnumerable<JobDTO>>();

        return Ok(dto);
    }

    [HttpGet("unscheduled")]
    public async Task<IActionResult> Unscheduled()
    {
        var jobs = await unitOfWork.Jobs.ListUnscheduledAsync();

        var dto = jobs.Adapt<IEnumerable<JobDTO>>();

        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> AddJob([FromBody] JobDTO jobDTO)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var validationResult = await jobValidationService.ValidateAsync(jobDTO);

        if (!validationResult.IsValid)
        {
            ModelState.AddValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        var newJob = jobDTO.Adapt<Job>();

        await unitOfWork.Jobs.AddAsync(newJob);
        await unitOfWork.SaveChangesAsync();

        logger.LogInformation("Quote created successfully.");
        return CreatedAtAction(nameof(AddJob), new { id = newJob.Id }, newJob);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchJob(int id, [FromBody] JsonPatchDocument<JobDTO> patchDocument)
    {
        if (patchDocument == null)
            return BadRequest();

        var job = await unitOfWork.Jobs.GetJobAsync(id);

        if (job == null)
            return NotFound();

        var jobDTO = job.Adapt<JobDTO>();

        patchDocument.ApplyTo(jobDTO, ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var validationResult = await jobValidationService.ValidateAsync(jobDTO);

        if (!validationResult.IsValid)
        {
            ModelState.AddValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        if (jobDTO.Title != job.Title) job.Title = jobDTO.Title;
        if (jobDTO.Start != job.Start) job.Start = jobDTO.Start;
        if (jobDTO.End != job.End) job.End = jobDTO.End;
        if (jobDTO.IsScheduled != job.IsScheduled) job.IsScheduled = jobDTO.IsScheduled;
        if (jobDTO.AllDay != job.AllDay) job.AllDay = jobDTO.AllDay;

        await unitOfWork.SaveChangesAsync();

        return NoContent();
    }
}
