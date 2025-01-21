using Moq;
using Microsoft.Extensions.Logging;
using BizTrak.Api;
using BizTrak.Application.Services;
using BizTrak.Data;
using Microsoft.AspNetCore.Mvc;
using BizTrak.DTO;
using BizTrak.Domain.Entities;
using System.Linq.Expressions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace BizTrak.UnitTests.Api.Controllers;

public class QuotesControllerTests
{
    private readonly Mock<ILogger<QuotesController>> _loggerMock;
    private readonly Mock<IQuoteService> _quoteServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IQuoteRepository> _quoteRepositoryMock;
    private readonly QuotesController _controller;

    private static readonly List<Quote> _testQuotes = new()
        {
            new Quote { Id = 1, QuoteRef = "Q-100", Title = "Test Quote 1", CustomerId = 1 },
            new Quote { Id = 2, QuoteRef = "Q-101", Title = "Test Quote 2", CustomerId = 1 },
            new Quote { Id = 3, QuoteRef = "Q-102", Title = "Test Quote 3", CustomerId = 1 }
        };

    private static readonly List<QuoteDTO> _testQuoteDTOs = new() {
            new QuoteDTO { Id = 1, QuoteRef = "Q-101", Title = "" },
            new QuoteDTO { Id = 2, QuoteRef = "Q-102", Title = "" }
        };

    public QuotesControllerTests()
    {
        _loggerMock = new Mock<ILogger<QuotesController>>();
        _quoteServiceMock = new Mock<IQuoteService>();
        _quoteRepositoryMock = new Mock<IQuoteRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        // Setup the IUnitOfWork to return the mock repository
        _unitOfWorkMock
            .Setup(uow => uow.Quotes)
            .Returns(_quoteRepositoryMock.Object);

        _controller = new QuotesController(
            _loggerMock.Object,
            _quoteServiceMock.Object,
            _unitOfWorkMock.Object
        );
    }

    #region GetQuoteById

    [Fact]
    public async Task GetQuoteById_WhenQuoteExists_ShouldReturnOkWithQuote()
    {
        // Arrange
        var quoteId = 1;
        var quote = _testQuotes.First(q => q.Id == quoteId);

        _quoteRepositoryMock
            .Setup(repo => repo.GetQuoteAsync(quoteId, true))
            .ReturnsAsync(quote);

        // Act
        var result = await _controller.GetQuoteById(quoteId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedQuote = Assert.IsType<QuoteDTO>(okResult.Value);
        Assert.Equal(quoteId, returnedQuote.Id);
    }

    [Fact]
    public async Task GetQuoteById_WhenQuoteDoesNotExist_ShouldReturnNotFound()
    {
        var quoteId = 1;
        _quoteRepositoryMock
            .Setup(repo => repo.GetQuoteAsync(quoteId, true))
            .ReturnsAsync((Quote)null!);

        var result = await _controller.GetQuoteById(quoteId);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    #endregion

    #region ListQuotes

    [Fact]
    public async Task ListQuotes_WhenCalled_ShouldReturnPagedQuotes()
    {
        // Arrange
        var quoteState = (QuoteState?)null;
        var page = 1;
        var pageSize = 10;
        var search = "";

        var pagedData = new PagedQuotes
        {
            Quotes = [_testQuotes.First()],
            Total = 1,
            Page = page,
            PageSize = pageSize
        };

        // Mock the repository call
        _quoteRepositoryMock
            .Setup(repo => repo.PagedQuotes(page, pageSize, It.IsAny<Expression<Func<Quote, bool>>>()))
            .ReturnsAsync(pagedData);

        // Act
        var result = await _controller.ListQuotes(quoteState, page, pageSize, search);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var data = Assert.IsType<PagedQuotesDTO>(okResult.Value);
        Assert.Equal(1, data.Total);
        Assert.Single(data.Quotes);
        Assert.Equal(page, data.Page);
        Assert.Equal(pageSize, data.PageSize);
    }

    #endregion

    #region CreateQuote

    [Fact]
    public async Task CreateQuote_WhenModelStateIsInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        var invalidQuoteDto = new QuoteDTO() { Title = "" };
        _controller.ModelState.AddModelError("Key", "ErrorMessage");

        // Act
        var result = await _controller.CreateQuote(invalidQuoteDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreateQuote_WhenValid_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var quoteDto = new QuoteDTO { Id = 0, QuoteRef = "Q-101", Title = "" };
        var createdQuote = new QuoteDTO { Id = 1, QuoteRef = "Q-101", Title = "" };

        _quoteServiceMock
            .Setup(service => service.CreateQuoteAsync(quoteDto))
            .ReturnsAsync(createdQuote);

        // Act
        var result = await _controller.CreateQuote(quoteDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("CreateQuote", createdResult.ActionName);
        var returnedQuote = Assert.IsType<QuoteDTO>(createdResult.Value);
        Assert.Equal(1, returnedQuote.Id);
    }

    #endregion

    #region UpdateQuote

    [Fact]
    public async Task UpdateQuote_WhenModelStateIsInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        var quoteDto = _testQuoteDTOs.First();
        _controller.ModelState.AddModelError("Key", "ErrorMessage");

        // Act
        var result = await _controller.UpdateQuote(1, quoteDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateQuote_WhenValid_ShouldReturnNoContent()
    {
        // Arrange
        var quoteDto = _testQuoteDTOs.First();

        _quoteServiceMock
            .Setup(service => service.UpdateQuoteAsync(1, quoteDto))
            .ReturnsAsync(quoteDto);

        // Act
        var result = await _controller.UpdateQuote(1, quoteDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    #endregion

    #region PatchQuote

    [Fact]
    public async Task PatchQuote_WhenPatchDocumentIsNull_ShouldReturnBadRequest()
    {
        // Arrange
        JsonPatchDocument<QuoteDTO> patchDoc = null!;

        // Act
        var result = await _controller.PatchQuote(1, patchDoc);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task PatchQuote_WhenQuoteNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var patchDoc = new JsonPatchDocument<QuoteDTO>();
        _quoteRepositoryMock
            .Setup(repo => repo.GetQuoteAsync(1, false))
            .ReturnsAsync((Quote)null!);

        // Act
        var result = await _controller.PatchQuote(1, patchDoc);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task PatchQuote_WhenValid_ShouldReturnNoContent()
    {
        // Arrange
        var existingQuote = _testQuotes.First();
        var patchDoc = new JsonPatchDocument<QuoteDTO>();
        patchDoc.Replace(q => q.QuoteRef, "Q-101");

        _quoteRepositoryMock
            .Setup(repo => repo.GetQuoteAsync(1, false))
            .ReturnsAsync(existingQuote);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _controller.PatchQuote(1, patchDoc);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Equal("Q-101", existingQuote.QuoteRef);
    }

    #endregion

    #region SendQuote

    [Fact]
    public async Task SendQuote_WhenQuoteNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var quoteId = 999;
        _quoteServiceMock
            .Setup(service => service.UpdateQuoteStatusAsync(quoteId, Trigger.Send))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.SendQuote(quoteId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Contains("not found", notFoundResult.Value.ToString()!);
    }

    [Fact]
    public async Task SendQuote_WhenValid_ShouldReturnOk()
    {
        // Arrange
        var quoteId = 1;
        _quoteServiceMock
            .Setup(service => service.UpdateQuoteStatusAsync(quoteId, Trigger.Send))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.SendQuote(quoteId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Contains("Quote status updated successfully", okResult.Value.ToString()!);
    }

    #endregion

    #region AcceptQuote

    [Fact]
    public async Task AcceptQuote_WhenQuoteNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var quoteId = 999;
        _quoteServiceMock
            .Setup(service => service.UpdateQuoteStatusAsync(quoteId, Trigger.Accept))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.AcceptQuote(quoteId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Contains("not found", notFoundResult.Value.ToString()!);
    }

    [Fact]
    public async Task AcceptQuote_WhenValid_ShouldReturnOk()
    {
        // Arrange
        var quoteId = 1;
        _quoteServiceMock
            .Setup(service => service.UpdateQuoteStatusAsync(quoteId, Trigger.Accept))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.AcceptQuote(quoteId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Contains("Quote status updated successfully", okResult.Value.ToString()!);
    }

    [Fact]
    public async Task AcceptQuote_WhenThrowsDbUpdateException_ShouldReturn500()
    {
        // Arrange
        var quoteId = 1;
        _quoteServiceMock
            .Setup(service => service.UpdateQuoteStatusAsync(quoteId, Trigger.Accept))
            .Throws<DbUpdateException>();

        // Act
        var result = await _controller.AcceptQuote(quoteId);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Contains("database error", objectResult.Value.ToString()!.ToLower());
    }

    #endregion

    #region RejectQuote

    [Fact]
    public async Task RejectQuote_WhenQuoteNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var quoteId = 999;
        _quoteServiceMock
            .Setup(service => service.UpdateQuoteStatusAsync(quoteId, Trigger.Reject))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.RejectQuote(quoteId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Contains("not found", notFoundResult.Value.ToString()!);
    }

    [Fact]
    public async Task RejectQuote_WhenValid_ShouldReturnOk()
    {
        // Arrange
        var quoteId = 1;
        _quoteServiceMock
            .Setup(service => service.UpdateQuoteStatusAsync(quoteId, Trigger.Reject))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.RejectQuote(quoteId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Contains("Quote status updated successfully", okResult.Value.ToString()!);
    }

    [Fact]
    public async Task RejectQuote_WhenThrowsInvalidOperationException_ShouldReturnConflict()
    {
        // Arrange
        var quoteId = 1;
        _quoteServiceMock
            .Setup(service => service.UpdateQuoteStatusAsync(quoteId, Trigger.Reject))
            .Throws(new InvalidOperationException("Sample error message"));

        // Act
        var result = await _controller.RejectQuote(quoteId);

        // Assert
        var conflictResult = Assert.IsType<ConflictObjectResult>(result);
        Assert.Contains("Sample error message", conflictResult.Value.ToString()!);
    }

    #endregion

    #region GetQuotePdf

    [Fact]
    public async Task GetQuotePdf_WhenQuoteNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var quoteId = 999;
        _quoteRepositoryMock
            .Setup(repo => repo.GetQuoteAsync(quoteId, true))
            .ReturnsAsync((Quote)null!);

        // Act
        var result = await _controller.GetQuotePdf(quoteId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    #endregion
}
