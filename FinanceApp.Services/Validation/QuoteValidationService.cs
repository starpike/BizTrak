namespace FinanceApp.Services;

using System.Threading.Tasks;
using FinanceApp.DTOs;

public interface IQuoteValidationService
{
    Task<ValidationResult> ValidateQuoteAsync(QuoteDTO quote);
}

public class QuoteValidationService : IQuoteValidationService
{
    public Task<ValidationResult> ValidateQuoteAsync(QuoteDTO quote)
    {
        var errors = new List<ValidationError>();

        if (!quote.QuoteDate.HasValue)
        {
            errors.Add(new ValidationError("QuoteDate", "Quote date has not been selected"));

        }
        else if (quote.QuoteDate < DateTime.UtcNow)
        {
            errors.Add(new ValidationError("QuoteDate", "Quote date cannot be before today"));
        }

        if (quote.ClientId <= 0)
        {
            errors.Add(new ValidationError("ClientId", "Please select a client"));
        }

        if (string.IsNullOrWhiteSpace(quote.QuoteTitle))
        {
            errors.Add(new ValidationError("QuoteTitle", "The quote must have a title"));
        }
        else if (quote.QuoteTitle.Length < 20)
        {
            errors.Add(new ValidationError("QuoteTitle", "Quote title should be more than 20 characters"));
        }

        if (errors.Count > 0)
        {
            return Task.FromResult(new ValidationResult(false, errors));
        }

        return Task.FromResult(new ValidationResult(true, null));
    }
}
