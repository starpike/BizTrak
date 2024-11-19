using FinanceApp.Data;
using FinanceApp.Domain;
using FinanceApp.Domain.Extensions;
using FinanceApp.DTO;
using Mapster;

namespace FinanceApp.Services;

public class QuoteService(IUnitOfWork unitOfWork,
    IQuoteValidationService quoteValidationService) : IQuoteService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IQuoteValidationService _quoteValidationService = quoteValidationService;

    public async Task<QuoteDTO> CreateQuoteAsync(QuoteDTO quoteDTO)
    {
        var validationResult = await _quoteValidationService.ValidateQuoteAsync(quoteDTO);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var quote = quoteDTO.Adapt<Quote>();

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            await _unitOfWork.Quotes.CreateQuoteAsync(quote);
            await _unitOfWork.SaveChangesAsync();

            quote.QuoteRef = quote.GenerateRef();
            quote.TotalAmount = quote.CalculateTotal();

            await _unitOfWork.SaveChangesAsync();

            var newQuoteDto = quote.ToDTO();

            await _unitOfWork.CommitTransactionAsync();

            return newQuoteDto;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<QuoteDTO> UpdateQuoteAsync(int id, QuoteDTO quoteDTO)
    {
        var validationResult = await _quoteValidationService.ValidateQuoteAsync(quoteDTO);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var quoteToUpdate = await _unitOfWork.Quotes.GetQuoteAsync(id, true) ??
            throw new ValidationException([new("Quote Id", "Quote not found")]);

        var updatedData = quoteDTO.Adapt<Quote>();
        _unitOfWork.Quotes.UpdateQuoteAsync(quoteToUpdate, updatedData);

        if (updatedData.Tasks != null)
        {
            // Clear existing tasks
            _unitOfWork.Quotes.RemoveTasks(quoteToUpdate.Tasks);
            quoteToUpdate.Tasks.Clear();

            foreach (var taskData in updatedData.Tasks)
            {
                quoteToUpdate.Tasks.Add(new QuoteTask
                {
                    QuoteId = quoteToUpdate.Id,
                    Description = taskData.Description,
                    EstimatedDuration = taskData.EstimatedDuration,
                    Cost = taskData.Cost,
                    OrderIndex = taskData.OrderIndex
                });
            }
        }

        if (updatedData.Materials != null)
        {
            _unitOfWork.Quotes.RemoveMaterials(quoteToUpdate.Materials);
            quoteToUpdate.Materials.Clear();

            foreach (var materialData in updatedData.Materials)
            {
                quoteToUpdate.Materials.Add(new QuoteMaterial
                {
                    QuoteId = quoteToUpdate.Id,
                    MaterialName = materialData.MaterialName,
                    Quantity = materialData.Quantity,
                    UnitPrice = materialData.UnitPrice
                });
            }
        }

        updatedData.TotalAmount = quoteToUpdate.CalculateTotal();
        _unitOfWork.Quotes.UpdateQuoteAsync(quoteToUpdate, updatedData);

        var updatedQuoteDto = quoteToUpdate.ToDTO();

        await _unitOfWork.SaveChangesAsync();

        return updatedQuoteDto;
    }

    public async Task<bool> UpdateQuoteStatusAsync(int quoteId, Trigger trigger)
    {
        var quote = await _unitOfWork.Quotes.GetQuoteAsync(quoteId, false);

        if (quote == null)
            return false;

        if (!quote.CanFire(trigger))
            throw new InvalidOperationException("Invalid state transition.");
        
        quote.Fire(trigger);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}

public interface IQuoteService
{
    public Task<QuoteDTO> CreateQuoteAsync(QuoteDTO quoteDTO);
    public Task<QuoteDTO> UpdateQuoteAsync(int id, QuoteDTO quoteDTO);
    Task<bool> UpdateQuoteStatusAsync(int quoteId, Trigger trigger);
}