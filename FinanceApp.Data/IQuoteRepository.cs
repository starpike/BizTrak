namespace FinanceApp.Data;

using FinanceApp.Domain;

public interface IQuoteRepository
{
    public Task<IEnumerable<Quote>> AllQuotesAsync();
    public Task<Quote> GetQuoteAsync(int id);
    public Task<PagedQuotes> PagedQuotes(int page, int pageSize, string search);
    public Task<Quote> CreateQuoteAsync(Quote quote);
}