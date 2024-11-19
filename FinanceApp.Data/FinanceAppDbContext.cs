namespace FinanceApp.Data;

using FinanceApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class FinanceAppDbContext : DbContext
{
    public FinanceAppDbContext() : base() { }
    public FinanceAppDbContext(DbContextOptions options) : base(options) { }
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Quote> Quotes { get; set; }
    public virtual DbSet<QuoteTask> QuoteTasks { get; set; }
    public virtual DbSet<QuoteMaterial> QuoteMaterials { get; set; }
    public virtual DbSet<Job> Jobs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Job>().HasKey(j => j.Id);

        modelBuilder.Entity<Job>().Property(j => j.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<Job>()
            .HasOne(j => j.Quote)
            .WithOne()
            .HasForeignKey<Job>(j => j.QuoteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<QuoteTask>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<QuoteTask>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<QuoteMaterial>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<QuoteMaterial>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Quote>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<Quote>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Quote>()
            .Property(e => e.QuoteDate)
            .HasConversion(
            d => d,
            d => DateTime.SpecifyKind(d, DateTimeKind.Utc)
        );

        modelBuilder.Entity<Quote>()
            .Property(e => e.UpdatedAt)
            .HasConversion(
            d => d,
            d => DateTime.SpecifyKind(d, DateTimeKind.Utc)
        );

        modelBuilder.Entity<Quote>()
            .Property(e => e.CreatedAt)
            .HasConversion(
            d => d,
            d => DateTime.SpecifyKind(d, DateTimeKind.Utc)
        );

        modelBuilder.Entity<Customer>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<Customer>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Quote>()
            .HasOne(q => q.Customer)
            .WithMany()
            .HasForeignKey(q => q.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}