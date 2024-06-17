
namespace FinanceApp.Domain;

public class Quote
{
    private DateTime quoteDate;

    public string GenerateUniqueReference(int quoteId)
    {
        // Current DateTime in a specific format
        string datePart = DateTime.UtcNow.ToString("yyyyMMdd");

        // Combine the date part and quote ID to form the unique reference
        return $"REF-{datePart}-{quoteId}";
    }

    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    public string? QuoteRef { get; set; }
    public string? QuoteTitle { get; set; }
    public DateTime QuoteDate
    {
        get => quoteDate;
        set => quoteDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
    public List<Job>? Jobs { get; set; }
}