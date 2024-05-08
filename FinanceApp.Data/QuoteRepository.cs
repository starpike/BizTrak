namespace FinanceApp.Data;

using FinanceApp.Domain;
using Microsoft.EntityFrameworkCore;


public class QuoteRepository(FinanceAppDbContext financeAppDbContext) : IQuoteRepository
{
    private readonly FinanceAppDbContext _financeAppDbContext = financeAppDbContext;

    public async Task<IEnumerable<Quote>> AllQuotesAsync()
    {
        return await _financeAppDbContext.Quotes.Include(q => q.Client).ToListAsync();
    }

    public async Task<Quote> GetQuoteAsync(int id)
    {
        var quote = await _financeAppDbContext.Quotes.FindAsync(id);
        return quote!;
    }
}