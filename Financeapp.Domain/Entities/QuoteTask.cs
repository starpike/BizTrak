using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceApp.Domain.Entities;
public class QuoteTask
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("QuoteId")]
        public Quote? Quote { get; set; }
        public required int QuoteId { get; set; }

        [Required]
        [StringLength(255)]
        public required string Description { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal EstimatedDuration { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Cost { get; set; }

        public int OrderIndex { get; set; }

        public override string ToString()
        {
            return $"Task {Id} for quote {QuoteId} - {Description}";
        }
    }
