using System;

namespace BizTrak.DTO;

public class JobDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public int QuoteId { get; set; }    
    public DateTime Start { get; set; } 
    public DateTime End { get; set; }   
    public string QuoteRef { get; set; } = string.Empty;
    public bool AllDay { get; set; } = false;
    public bool IsScheduled { get; set; }
}
