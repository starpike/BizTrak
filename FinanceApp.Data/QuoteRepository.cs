namespace FinanceApp.Data;

using System.Linq.Expressions;
using FinanceApp.Domain.Entities;
using FinanceApp.DTO;
using Microsoft.EntityFrameworkCore;

public class QuoteRepository(FinanceAppDbContext financeAppDbContext) : IQuoteRepository
{
    private readonly FinanceAppDbContext _financeAppDbContext = financeAppDbContext;

    public async Task<IEnumerable<Quote>> ListQuotesAsync()
    {
        return await _financeAppDbContext.Quotes
            .AsNoTracking()
            .Include(q => q.Customer)
            .ToListAsync();
    }

    public async Task<PagedQuotes> PagedQuotes(int page, int pageSize, Expression<Func<Quote, bool>> filter)
    {
        var filteredQuotes = _financeAppDbContext.Quotes.Include(q => q.Customer)
                    .AsNoTracking()
                    .AsQueryable().Where(filter);
        var total = await filteredQuotes.CountAsync();
        var quotes = await filteredQuotes.OrderByDescending(q => q.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        var pagedQuotes = new PagedQuotes
        {
            Total = total,
            Quotes = quotes,
            Page = page,
            PageSize = pageSize
        };

        return pagedQuotes;
    }

    public async Task<Quote> GetQuoteAsync(int id, bool includeRelated = false)
    {
        IQueryable<Quote> query = _financeAppDbContext.Quotes
            .Include(q => q.Customer);

        if (includeRelated)
        {
            query = query
                .Include(q => q.Materials)
                .Include(q => q.Tasks);
        }

        var quote = await query.FirstOrDefaultAsync(q => q.Id == id);

        if (quote != null && includeRelated)
        {
            quote.Tasks = quote.Tasks.OrderBy(t => t.OrderIndex).ToList();
        }

        return quote!;
    }

    public async Task<Quote> AddAsync(Quote quote)
    {
        if (quote == null)
            throw new ArgumentNullException(nameof(quote), "Quote cannot be null");


        await _financeAppDbContext.Quotes.AddAsync(quote);
        return quote;
    }

    public void UpdateAsync(Quote quote, Quote updatedData)
    {
        if (quote == null)
            throw new ArgumentNullException(nameof(quote), "Quote cannot be null");

        _financeAppDbContext.Entry(quote).CurrentValues.SetValues(updatedData);
    }

    public void RemoveTasks(IEnumerable<QuoteTask> tasks)
    {
        _financeAppDbContext.QuoteTasks.RemoveRange(tasks);
    }

    public void RemoveMaterials(IEnumerable<QuoteMaterial> materials)
    {
        _financeAppDbContext.QuoteMaterials.RemoveRange(materials);
    }

    public bool QuoteExists(int id)
    {
        return _financeAppDbContext.Quotes.Any(q => q.Id == id);
    }
}