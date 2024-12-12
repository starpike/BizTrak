
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FinanceApp.Domain.Events;

namespace FinanceApp.Domain.Entities;

public class Quote : BaseEntity
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    public string QuoteRef { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public required string Title { get; set; }

    [ForeignKey("CustomerId")]
    public Customer? Customer { get; set; }
    public required int CustomerId { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime QuoteDate { get; set; } = DateTime.Now.ToUniversalTime();

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; } = 0.00m;

    [Required]
    public QuoteState State { get; private set; } = QuoteState.Draft;

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; } = DateTime.Now.ToUniversalTime();

    public DateTime UpdatedAt { get; set; } = DateTime.Now.ToUniversalTime();

    public virtual ICollection<QuoteTask> Tasks { get; set; } = [];
    public virtual ICollection<QuoteMaterial> Materials { get; set; } = [];

    public decimal CalculateTotal()
    {
        var tasksTotal = Tasks?.Sum(t => t.Cost) ?? 0m;
        var materialsTotal = Materials?.Sum(m => m.Quantity * m.UnitPrice) ?? 0m;
        return tasksTotal + materialsTotal;
    }

    public string GenerateRef()
    {
        return $"QUO-{QuoteDate:yyyyMMdd}-{Id}";
    }

    private readonly Dictionary<(QuoteState, Trigger), QuoteState> _transitions = new()
    {
            { (QuoteState.Draft, Trigger.Send), QuoteState.Sent },
            { (QuoteState.Sent, Trigger.Accept), QuoteState.Accepted },
            { (QuoteState.Sent, Trigger.Reject), QuoteState.Rejected }
    };

    public bool CanFire(Trigger trigger)
    {
        return _transitions.ContainsKey((State, trigger));
    }

    public void Fire(Trigger trigger)
    {
        if (!CanFire(trigger))
        {
            throw new InvalidOperationException($"Cannot perform '{trigger}' in state '{State}'.");
        }

        State = _transitions[(State, trigger)];

        if (State.Equals(QuoteState.Accepted))
             RaiseDomainEvent(new QuoteAcceptedEvent(this));
    }

    public override string ToString()
    {
        return $"Quote {Id} for {Customer?.Name}";
    }
}
