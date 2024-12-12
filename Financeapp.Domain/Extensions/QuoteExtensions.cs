using FinanceApp.Domain.Entities;
using FinanceApp.DTO;

namespace FinanceApp.Domain.Extensions;
public static class QuoteExtensions
{
    public static QuoteDTO ToDTO(this Quote quote)
    {
        return new QuoteDTO
        {
            Id = quote.Id,
            QuoteRef = quote.QuoteRef,
            Title = quote.Title,
            TotalAmount = quote.TotalAmount,
            State = (int)quote.State,
            CustomerId = quote.CustomerId,
            CustomerDetail = quote.Customer?.ToDTO(),
            Tasks = quote.Tasks.Select(t => new QuoteTaskDTO
            {
                Id = t.Id,
                QuoteId = t.QuoteId,
                Description = t.Description,
                EstimatedDuration = t.EstimatedDuration,
                Cost = t.Cost,
                OrderIndex = t.OrderIndex
            }).ToList(),
            Materials = quote.Materials.Select(m => new QuoteMaterialDTO
            {
                Id = m.Id,
                QuoteId = m.QuoteId,
                MaterialName = m.MaterialName,
                Quantity = m.Quantity,
                UnitPrice = m.UnitPrice,
                TotalPrice = m.TotalPrice
            }).ToList()
        };
    }

    public static List<QuoteDTO> ToDTOList(this IEnumerable<Quote> quotes)
    {
        return quotes.Select(q => q.ToDTO()).ToList();
    }
}
