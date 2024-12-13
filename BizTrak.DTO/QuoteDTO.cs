namespace BizTrak.DTO;

public class QuoteDTO
{
    public int Id { get; set; }
    public string QuoteRef { get; set; } = "";
    public required string Title { get; set; }
    public int CustomerId { get; set; }
    public CustomerDTO? CustomerDetail { get; set; }
    public decimal TotalAmount { get; set; }
    public int State { get; set; }
    public List<QuoteTaskDTO> Tasks { get; set; } = [];
    public List<QuoteMaterialDTO> Materials { get; set; } = [];
}