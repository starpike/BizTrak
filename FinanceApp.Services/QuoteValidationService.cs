namespace FinanceApp.Services;

using System.Threading.Tasks;
using FinanceApp.Domain;

public interface IQuoteValidationService
{
    Task<bool> ValidateQuoteAsync(Quote quote);
}

public class QuoteValidationService : IQuoteValidationService
{
    public Task<bool> ValidateQuoteAsync(Quote quote)
    {
        if (quote.QuoteDate < DateTime.UtcNow)
        {
            return Task.FromResult(false);
        }

        return Task.FromResult(true);
    }
}
