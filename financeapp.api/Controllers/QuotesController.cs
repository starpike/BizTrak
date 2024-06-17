namespace FinanceApp.Api;
using FinanceApp.Data;
using FinanceApp.Domain;
using FinanceApp.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class QuotesController(ILogger<QuotesController> logger, IQuoteService quoteService, IQuoteRepository quoteRepository) : ControllerBase
{
    private readonly ILogger<QuotesController> logger = logger;
    private readonly IQuoteService quoteService = quoteService;
    private readonly IQuoteRepository quoteRepository = quoteRepository;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Job>>> GetQuotes()
    {
        var quotes = await quoteRepository.AllQuotesAsync();
        return Ok(quotes);
    }

    [HttpGet]
    [Route("search")]
    public async Task<IActionResult> GetQuotes([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = "")
    {
        var pagedQuotes = await quoteRepository.PagedQuotes(page, pageSize, search);

        var response = new
        {
            Data = pagedQuotes.Quotes,
            TotalCount = pagedQuotes.Total,
            Page = page,
            PageSize = pageSize
        };

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateQuote([FromBody] Quote quote)
    {
        try
        {
            var createdQuote = await quoteService.CreateQuoteAsync(quote);
            logger.LogInformation("Quote created successfully.");
            return CreatedAtAction(nameof(CreateQuote), new { id = createdQuote.Id }, createdQuote);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating the quote.");
            return StatusCode(500, "Internal server error");
        }
    }
}
