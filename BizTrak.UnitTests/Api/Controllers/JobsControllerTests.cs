using System;
using BizTrak.Api.Controllers;
using BizTrak.Application.Validation;
using BizTrak.Data;
using BizTrak.Domain.Entities;
using BizTrak.DTO;
using Mapster;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BizTrak.UnitTests.Api.Controllers;

public class JobsControllerTests
{
    private readonly Mock<ILogger<JobsController>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IJobValidationService> _validationServiceMock;
    private readonly JobsController _controller;

    public JobsControllerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<JobsController>>();
        _validationServiceMock = new Mock<IJobValidationService>();
        _controller = new JobsController(_loggerMock.Object, _unitOfWorkMock.Object, _validationServiceMock.Object);
    }

    [Fact]
    public async Task Scheduled_ReturnsOk_WithJobsWithinDateRange()
    {
        var start = new DateTime(2025, 1, 1);
        var end = new DateTime(2025, 1, 31);

        var jobs = new List<Job>
        {
            new() { Id = 1, QuoteId = 1, Title = "", Start = start, End = end, CustomerName = "" },
            new() { Id = 2, QuoteId = 2, Title = "", Start = start, End = end, CustomerName = "" }
        };

        _unitOfWorkMock.Setup(u => u.Jobs.ListScheduledAsync(start, end)).ReturnsAsync(jobs);

        // Act
        var result = await _controller.Scheduled(start, end);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedJobs = Assert.IsAssignableFrom<IEnumerable<JobDTO>>(okResult.Value);
        Assert.Equal(2, returnedJobs.Count());
    }

    [Fact]
    public async Task Scheduled_ReturnsOk_WithEmptyList_WhenNoJobsFound()
    {
        // Arrange
        var start = new DateTime(2025, 1, 1);
        var end = new DateTime(2025, 1, 31);

        _unitOfWorkMock.Setup(u => u.Jobs.ListScheduledAsync(start, end)).ReturnsAsync(new List<Job>());

        // Act
        var result = await _controller.Scheduled(start, end);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedJobs = Assert.IsAssignableFrom<IEnumerable<JobDTO>>(okResult.Value);
        Assert.Empty(returnedJobs);
    }

    [Fact]
    public async Task Scheduled_ThrowsException_WhenUnitOfWorkFails()
    {
        // Arrange
        var start = new DateTime(2025, 1, 1);
        var end = new DateTime(2025, 1, 31);

        _unitOfWorkMock.Setup(u => u.Jobs.ListScheduledAsync(start, end)).ThrowsAsync(new Exception("Database failure"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(async () => await _controller.Scheduled(start, end));
    }

    [Fact]
    public async Task UnScheduled_Returns_Unscheduled_Jobs()
    {
        var jobs = new List<Job>()
        {
            new() { Id = 1, QuoteId = 1, Title = "", CustomerName = "" },
            new() { Id = 2, QuoteId = 2, Title = "", CustomerName = "" }
        };

        _unitOfWorkMock.Setup(u => u.Jobs.ListUnscheduledAsync()).ReturnsAsync(jobs);

        var result = await _controller.Unscheduled();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedJobs = Assert.IsAssignableFrom<IEnumerable<JobDTO>>(okResult.Value);
        Assert.Equal(2, returnedJobs.Count());
    }

    [Fact]
    public async Task UnScheduled_ReturnsOk_WithEmptyList_WhenNoJobsFound()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.Jobs.ListUnscheduledAsync()).ReturnsAsync(new List<Job>());

        // Act
        var result = await _controller.Unscheduled();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedJobs = Assert.IsAssignableFrom<IEnumerable<JobDTO>>(okResult.Value);
        Assert.Empty(returnedJobs);
    }

    [Fact]
    public async Task UnScheduled_ThrowsException_WhenUnitOfWorkFails()
    {
        _unitOfWorkMock.Setup(u => u.Jobs.ListUnscheduledAsync()).ThrowsAsync(new Exception("Database failure"));

        await Assert.ThrowsAsync<Exception>(async () => await _controller.Unscheduled());
    }

    [Fact]
    public async Task AddJob_ReturnsBadRequest_WhenModelStateInvalid()
    {
        _controller.ModelState.AddModelError("Title", "Required");

        var result = await _controller.AddJob(new JobDTO());

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task AddJob_ReturnsBadRequest_WhenValidationFails()
    {
        _validationServiceMock.Setup(u => u.ValidateAsync(It.IsAny<JobDTO>()))
            .ReturnsAsync(new ValidationResult(false, []));

        var result = await _controller.AddJob(new JobDTO());

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task AddJob_ReturnsCreatedAtAction_WhenAddingAndSavingJob()
    {
        var jobDTO = new JobDTO()
        {
            Id = 1,
            QuoteId = 1,
            Title = "",
            CustomerName = ""
        };

        _validationServiceMock.Setup(u => u.ValidateAsync(It.IsAny<JobDTO>()))
            .ReturnsAsync(new ValidationResult(true, []));

        _unitOfWorkMock.Setup(u => u.Jobs.AddAsync(It.IsAny<Job>()))
            .ReturnsAsync(jobDTO.Adapt<Job>());
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync());

        var result = await _controller.AddJob(jobDTO);

        _unitOfWorkMock.Verify(u => u.Jobs.AddAsync(It.IsAny<Job>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("AddJob", createdResult.ActionName);
        var returnedJob = Assert.IsType<Job>(createdResult.Value);
        Assert.Equal(1, returnedJob.Id);
    }

    [Fact]
    public async Task PatchJob_ReturnsBadRequest_WhenPatchDocumentIsNull()
    {
        // Act
        var result = await _controller.PatchJob(1, null!);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task PatchJob_ReturnsNotFound_WhenJobDoesNotExist()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.Jobs.GetJobAsync(1)).ReturnsAsync((Job)null!);

        // Act
        var result = await _controller.PatchJob(1, new JsonPatchDocument<JobDTO>());

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task PatchJob_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        var job = new Job { Id = 1, Title = "Title", QuoteId=1, CustomerName="" };
        _unitOfWorkMock.Setup(u => u.Jobs.GetJobAsync(1)).ReturnsAsync(job);

        var patchDocument = new JsonPatchDocument<JobDTO>();
        patchDocument.Replace(j => j.Title, null); // Invalid title

        _controller.ModelState.AddModelError("Title", "Title is required.");

        // Act
        var result = await _controller.PatchJob(1, patchDocument);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task PatchJob_ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange
        var job = new Job { Id = 1, QuoteId=1, Title = "Title", CustomerName="" };
        _unitOfWorkMock.Setup(u => u.Jobs.GetJobAsync(1)).ReturnsAsync(job);

        var patchDocument = new JsonPatchDocument<JobDTO>();
        patchDocument.Replace(j => j.Title, "Invalid Title");

        var validationResult = new ValidationResult(false, []);

        _validationServiceMock.Setup(v => v.ValidateAsync(It.IsAny<JobDTO>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _controller.PatchJob(1, patchDocument);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

        [Fact]
    public async Task PatchJob_ReturnsNoContent_WhenUpdateIsSuccessful()
    {
        // Arrange
        var job = new Job { Id = 1, QuoteId=1, Title = "Original Title", CustomerName="", Start = DateTime.Now, End = DateTime.Now.AddHours(1) };
        _unitOfWorkMock.Setup(u => u.Jobs.GetJobAsync(1)).ReturnsAsync(job);

        var patchDocument = new JsonPatchDocument<JobDTO>();
        patchDocument.Replace(j => j.Title, "Updated Title");

        var validationResult = new ValidationResult(true, []);
        _validationServiceMock.Setup(v => v.ValidateAsync(It.IsAny<JobDTO>()))
            .ReturnsAsync(validationResult);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync());

        // Act
        var result = await _controller.PatchJob(1, patchDocument);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}
