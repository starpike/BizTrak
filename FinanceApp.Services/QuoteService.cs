using FinanceApp.Data;
using FinanceApp.Domain;
using FinanceApp.DTOs;

namespace FinanceApp.Services;

public class QuoteService(IQuoteRepository quoteRepository, IClientRepository clientRepository, IQuoteValidationService quoteValidationService) : IQuoteService
{
    private readonly IQuoteRepository _quoteRepository = quoteRepository;
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IQuoteValidationService _quoteValidationService = quoteValidationService;

    public async Task<Quote> CreateQuoteAsync(QuoteDTO quoteDTO)
    {
        var validationResult = await _quoteValidationService.ValidateQuoteAsync(quoteDTO);
        
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var quote = new Quote {
            ClientId = quoteDTO.ClientId ?? 0,
            QuoteTitle = quoteDTO.QuoteTitle,
            QuoteDate = quoteDTO.QuoteDate ?? DateTime.MinValue
        };

        return await _quoteRepository.CreateQuoteAsync(quote);
    }
}

public interface IQuoteService
{
    public Task<Quote> CreateQuoteAsync(QuoteDTO quoteDTO);
}