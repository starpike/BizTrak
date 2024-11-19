using System;

namespace FinanceApp.DTO;

public class PagedQuotesDTO
{
    private IEnumerable<QuoteDTO> quotes = new List<QuoteDTO>();
    public IEnumerable<QuoteDTO> Quotes { get => quotes; set => quotes = value; }
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
