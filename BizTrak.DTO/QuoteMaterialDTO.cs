using System;

namespace BizTrak.DTO;

public class QuoteMaterialDTO
{
    public int Id { get; set;}
    public int QuoteId { get; set;}
    public required string MaterialName { get; set; }
    public long Quantity { get; set; }
    public decimal UnitPrice { get; set; } 
    public decimal TotalPrice { get; set; }
}
