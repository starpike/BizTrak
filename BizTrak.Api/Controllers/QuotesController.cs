namespace BizTrak.Api;

using BizTrak.Data;
using BizTrak.DTO;
using BizTrak.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using BizTrak.Domain.Extensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using BizTrak.Application.Services;
using Mapster;

[Route("api/[controller]")]
[ApiController]
public class QuotesController(ILogger<QuotesController> logger, IQuoteService quoteService, IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly ILogger<QuotesController> logger = logger;
    private readonly IQuoteService quoteService = quoteService;
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetQuoteById(int id)
    {
        var quote = await unitOfWork.Quotes.GetQuoteAsync(id, true);
        return Ok(quote.ToDTO());
    }

    [HttpGet]
    public async Task<IActionResult> ListQuotes([FromQuery] QuoteState? quoteState, int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = "")
    {
        var filter = QuoteFilterBuilder.BuildFilter(quoteState, search);

        var pagedQuotes = await unitOfWork.Quotes.PagedQuotes(page, pageSize, filter);

        var dto = new PagedQuotesDTO
        {
            Total = pagedQuotes.Total,
            Quotes = pagedQuotes.Quotes.ToDTOList(),
            Page = pagedQuotes.Page,
            PageSize = pagedQuotes.PageSize
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateQuote([FromBody] QuoteDTO quote)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var newQuote = await quoteService.CreateQuoteAsync(quote);
        logger.LogInformation("Quote created successfully.");
        return CreatedAtAction(nameof(CreateQuote), new { id = newQuote.Id }, newQuote);

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuote(int id, [FromBody] QuoteDTO quoteDTO)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        await quoteService.UpdateQuoteAsync(id, quoteDTO);
        logger.LogInformation("Quote updated successfully.");
        return NoContent();

    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchQuote(int id, [FromBody] JsonPatchDocument<QuoteDTO> patchDocument)
    {
        if (patchDocument == null)
            return BadRequest();

        var quote = await unitOfWork.Quotes.GetQuoteAsync(id);

        if (quote == null)
            return NotFound();

        var quoteDTO = quote.Adapt<QuoteDTO>();

        patchDocument.ApplyTo(quoteDTO, ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        quoteDTO.Adapt(quote);

        try
        {
            await unitOfWork.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!unitOfWork.Quotes.QuoteExists(id))
        {
            return NotFound();
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "A database error occurred while updating the quote.");
            return StatusCode(500, "A database error occurred. Please try again later.");
        }

        return NoContent();
    }

    [HttpPost("{quoteId}/send")]
    public async Task<IActionResult> SendQuote(int quoteId)
    {
        try
        {
            var result = await quoteService.UpdateQuoteStatusAsync(quoteId, Trigger.Send);

            if (!result)
                return NotFound($"Quote with ID {quoteId} not found.");

            return Ok(new { Message = "Quote status updated successfully." });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { Error = ex.Message });
        }
    }

    [HttpPost("{quoteId}/accept")]
    public async Task<IActionResult> AcceptQuote(int quoteId)
    {
        try
        {
            var result = await quoteService.UpdateQuoteStatusAsync(quoteId, Trigger.Accept);

            if (!result)
                return NotFound($"Quote with ID {quoteId} not found.");

            return Ok(new { Message = "Quote status updated successfully." });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { Error = ex.Message });
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "A database error occurred while updating the quote status.");
            return StatusCode(500, "A database error occurred. Please try again later.");
        }
    }

    [HttpPost("{quoteId}/reject")]
    public async Task<IActionResult> RejectQuote(int quoteId)
    {
        try
        {
            var result = await quoteService.UpdateQuoteStatusAsync(quoteId, Trigger.Reject);

            if (!result)
                return NotFound($"Quote with ID {quoteId} not found.");

            return Ok(new { Message = "Quote status updated successfully." });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { Error = ex.Message });
        }
    }

    [HttpGet("{id}/pdf")]
    public async Task<IActionResult> GetQuotePdf(int id)
    {
        var quote = await unitOfWork.Quotes.GetQuoteAsync(id, true);

        if (quote == null)
            return NotFound();

        var pdfBytes = QuotePdfGenerator.GenerateQuotePdf(quote);
        Response.Headers.Append("Content-Disposition", "inline; $filename={quote.QuoteRef}.pdf");
        return File(pdfBytes, "application/pdf");
    }
}

