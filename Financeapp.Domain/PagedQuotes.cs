namespace FinanceApp.Domain;

public class PagedQuotes {
    public IEnumerable<Quote> Quotes { get; set; }
    public int Total { get; set; }
}