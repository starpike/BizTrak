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
}
