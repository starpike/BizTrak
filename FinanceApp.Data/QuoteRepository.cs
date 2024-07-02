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

        var dto = new PagedQuotes
        {
            Total = await query.CountAsync()
        };

        dto.Quotes = await query
            .OrderByDescending(q => q.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return dto;
    }

    public async Task<Quote> GetQuoteAsync(int id)
    {
        var quote = await _financeAppDbContext.Quotes.Include(q => q.Client).FirstOrDefaultAsync(q => q.Id == id);
        return quote!;
    }

    public async Task<Quote> CreateQuoteAsync(Quote quote)
    {
        if (quote == null)
        {
            throw new ArgumentNullException(nameof(quote), "Quote cannot be null");
        }

        try
        {
            _financeAppDbContext.Quotes.Add(quote);
            await _financeAppDbContext.SaveChangesAsync();
            quote.QuoteRef = quote.GenerateUniqueReference(quote.Id);
            await _financeAppDbContext.SaveChangesAsync();
            return quote;
        }
        catch (DbUpdateException dbEx)
        {
            throw new InvalidOperationException("An error occurred while saving the quote to the database.", dbEx);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An unexpected error occurred while creating the quote.", ex);
        }
    }

}