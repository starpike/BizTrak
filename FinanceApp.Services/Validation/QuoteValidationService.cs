namespace FinanceApp.Services;

using System.Threading.Tasks;
using FinanceApp.DTO;

public interface IQuoteValidationService
{
    Task<ValidationResult> ValidateQuoteAsync(QuoteDTO quote);
}

public class QuoteValidationService : IQuoteValidationService
{
    public Task<ValidationResult> ValidateQuoteAsync(QuoteDTO quote)
    {
        var errors = new List<ValidationError>();

        if (quote.CustomerId <= 0)
        {
            errors.Add(new ValidationError("ClientId", "Please select a client"));
        }

        if (string.IsNullOrWhiteSpace(quote.Title))
        {
            errors.Add(new ValidationError("QuoteTitle", "The quote must have a title"));
        }
        else if (quote.Title.Length < 10)
        {
            errors.Add(new ValidationError("QuoteTitle", "Quote title should be more than 20 characters"));
        }

        return Task.FromResult(new ValidationResult(errors.Count == 0, errors));
    }
}
