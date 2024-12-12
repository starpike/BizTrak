namespace FinanceApp.Data;

using System.Linq.Expressions;
using FinanceApp.Domain.Entities;

public interface IQuoteRepository
{
    public Task<IEnumerable<Quote>> ListQuotesAsync();
    public Task<Quote> GetQuoteAsync(int id, bool includeRelated = false);
    public Task<PagedQuotes> PagedQuotes(int page, int pageSize, Expression<Func<Quote, bool>> filter);
    public Task<Quote> AddAsync(Quote quote);
    public void UpdateAsync(Quote quote, Quote updatedData);
    void RemoveTasks(IEnumerable<QuoteTask> tasks);
    void RemoveMaterials(IEnumerable<QuoteMaterial> materials);
    bool QuoteExists(int id);
}