using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceApp.Domain;

public class QuoteMaterial
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("QuoteId")]
        public Quote? Quote { get; set; }
        public int QuoteId { get; set; }

        [Required]
        [StringLength(255)]
        public required string MaterialName { get; set; }

        [Range(1, 1000)]
        public long Quantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; } = 0.00m;

        [NotMapped]
        public decimal TotalPrice => Quantity * UnitPrice;

        public override string ToString()
        {
            return $"Material {Id} for quote {QuoteId} - {MaterialName}";
        }
    }
