using System;

namespace BizTrak.DTO;

public class QuoteTaskDTO
{
    public int Id { get; set;}
    public int QuoteId { get; set;}
    public string Description { get; set; } = "";
    public decimal EstimatedDuration { get; set; }
    public Decimal Cost { get; set; }
    public int OrderIndex { get; set; }
}
