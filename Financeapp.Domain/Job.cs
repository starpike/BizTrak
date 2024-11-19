using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceApp.Domain;

public class Job
{
    [Key]
    public int Id { get; set;}

    [Required]
    public required int QuoteId { get; set; }

    [ForeignKey("QuoteId")]
    public Quote? Quote { get; set; }

    [Required]
    [StringLength(255)]
    public required string Title { get; set; }

    [Required]
    public DateTime Start { get; set; }

    [Required]
    public DateTime End { get; set; }

    public bool AllDay { get; set; } = false;
}
