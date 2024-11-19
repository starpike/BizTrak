using System;

namespace FinanceApp.DTO;

public class JobDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int QuoteId { get; set; }    
    public DateTime Start { get; set; } 
    public DateTime End { get; set; }   
    public bool AllDay { get; set; } = false;
}
