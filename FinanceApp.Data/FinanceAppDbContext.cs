namespace FinanceApp.Data;

using FinanceApp.Domain;
using Microsoft.EntityFrameworkCore;

public class FinanceAppDbContext : DbContext
{
    public FinanceAppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Quote> Quotes { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<User>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Job>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<Job>()
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
            d => d, // No conversion needed as we ensure it is UTC
            d => DateTime.SpecifyKind(d, DateTimeKind.Utc)
        );

        modelBuilder.Entity<Invoice>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<Invoice>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Expense>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<Expense>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Client>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<Client>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();
    }
}