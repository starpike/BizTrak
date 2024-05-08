namespace FinanceApp.Data;

using FinanceApp.Domain;

public interface IQuoteRepository
{
    public Task<IEnumerable<Quote>> AllQuotesAsync();
    public Task<Quote> GetQuoteAsync(int id);

}