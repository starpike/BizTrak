using FinanceApp.Data;
using FinanceApp.Domain;

namespace FinanceApp.Services;

public class QuoteService(IQuoteRepository quoteRepository, IClientRepository clientRepository, IQuoteValidationService quoteValidationService) : IQuoteService
{
    private readonly IQuoteRepository _quoteRepository = quoteRepository;
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IQuoteValidationService _quoteValidationService = quoteValidationService;

    public async Task<Quote> CreateQuoteAsync(Quote quote)
    {
        if (!await _quoteValidationService.ValidateQuoteAsync(quote))
        {
            throw new ArgumentException("Quote validation failed.");
        }

        await _quoteRepository.CreateQuoteAsync(quote);
        return quote;
    }
}

public interface IQuoteService
{
    public Task<Quote> CreateQuoteAsync(Quote quote);
}