namespace FinanceApp.Data;

using System.Linq.Expressions;
using FinanceApp.Domain;

public interface IQuoteRepository
{
    public Task<IEnumerable<Quote>> ListQuotesAsync();
    public Task<Quote> GetQuoteAsync(int id, bool includeRelated = false);
    public Task<PagedQuotes> PagedQuotes(int page, int pageSize, Expression<Func<Quote, bool>> filter);
    public Task<Quote> CreateQuoteAsync(Quote quote);
    public void UpdateQuoteAsync(Quote quote, Quote updatedData);
    void RemoveTasks(IEnumerable<QuoteTask> tasks);
    void RemoveMaterials(IEnumerable<QuoteMaterial> materials);
    bool QuoteExists(int id);
}