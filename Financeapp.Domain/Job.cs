namespace FinanceApp.Domain;
public class Job {
    public int Id { get; set;}
    public int? QuoteId { get; set; }
    public Quote? Quote { get; set; }
    public int? InvoiceId { get; set; }
    public Invoice? Invoice { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal EstimatedCost { get; set; }
    public decimal ActualCost { get; set; }
}