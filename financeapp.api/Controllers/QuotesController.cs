namespace FinanceApp.Api;
using FinanceApp.Data;
using FinanceApp.Domain;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class QuotesController(IQuoteRepository quoteRepository) : ControllerBase
{
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
}
