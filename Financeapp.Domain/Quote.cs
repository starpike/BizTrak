
namespace FinanceApp.Domain;

public class Quote
{
    public Quote()
    {
        QuoteRef = generateQuoteReference();
    }

    private string generateQuoteReference()
    {
        Guid newGuid = Guid.NewGuid();
        string encoded = Convert.ToBase64String(newGuid.ToByteArray());
        encoded = encoded.Replace("/", "_").Replace("+", "-"); // URL safe characters
        return encoded.Substring(0, 8);
    }

    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    public string QuoteRef { get; set; }
    public string? QuoteTitle { get; set; }
    public DateTime QuoteDate { get; set; }
    public List<Job>? Jobs { get; set; }
}