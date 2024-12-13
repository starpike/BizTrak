namespace BizTrak.Data;

using BizTrak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class BizTrakDbContext : DbContext
{
    public BizTrakDbContext() : base() { }
    public BizTrakDbContext(DbContextOptions options) : base(options) { }
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Quote> Quotes { get; set; }
    public virtual DbSet<QuoteTask> QuoteTasks { get; set; }
    public virtual DbSet<QuoteMaterial> QuoteMaterials { get; set; }
    public virtual DbSet<Job> Jobs { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<Role>().HasKey(r => r.Id);
        modelBuilder.Entity<Role>().Property(r => r.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

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