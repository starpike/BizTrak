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

    public async Task<PagedQuotes> PagedQuotes(int page, int pageSize, string search)
    {
        var query = _financeAppDbContext.Quotes.Include(q => q.Client).AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(q => q.QuoteRef.Contains(search) || q.QuoteTitle.Contains(search));
        }

        var dto = new PagedQuotes {
            Total = await query.CountAsync()
        };

        dto.Quotes = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return dto;
    }

    public async Task<Quote> GetQuoteAsync(int id)
    {
        var quote = await _financeAppDbContext.Quotes.FindAsync(id);
        return quote!;
    }
}