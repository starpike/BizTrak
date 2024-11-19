using FinanceApp.DTO;

namespace FinanceApp.Domain;

public class PagedQuotes {
    private IEnumerable<Quote> quotes = new List<Quote>();
    public IEnumerable<Quote> Quotes { get => quotes; set => quotes = value; }
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}