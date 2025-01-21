using System;
using BizTrak.Data;
using BizTrak.Domain.Entities;

namespace BizTrak.UnitTests.Data;

public class InMemoryData
{
    public static Customer Customer1 => new()
    {
        Id = 101,
        Name = "Test Customer 1",
        Address = ""
    };

    public static Customer Customer2 => new()
    {
        Id = 102,
        Name = "Test Customer 2",
        Address = ""
    };

    public static QuoteTask Task1 => new()
    {
        Id = 1,
        QuoteId = 1,
        Description = "First Task"
    };

    public static QuoteTask Task2 => new()
    {
        Id = 2,
        QuoteId = 1,
        Description = "Second Task"
    };

    public static QuoteMaterial Material1 => new()
    {
        Id = 1,
        QuoteId = 1,
        MaterialName = "First Material"
    };

    public static QuoteMaterial Material2 => new()
    {
        Id = 2,
        QuoteId = 1,
        MaterialName = "Second Material"
    };

    public static Quote Quote1 => new()
    {
        Id = 1,
        QuoteRef = "Q-100",
        Title = "Quote1 Title",
        CustomerId = Customer1.Id,
    };

    public static Quote Quote2 => new()
    {
        Id = 2,
        QuoteRef = "Q-200",
        Title = "Quote2 Title",
        CustomerId = Customer2.Id,
    };

    public static Quote Quote3 => new()
    {
        Id = 3,
        QuoteRef = "Q-300",
        Title = "Quote3 Title",
        CustomerId = Customer1.Id,
    };

    public static Quote Quote4 => new()
    {
        Id = 4,
        QuoteRef = "Q-400",
        Title = "Quote4 Title",
        CustomerId = Customer2.Id
    };

    public static Quote Quote5 => new()
    {
        Id = 5,
        QuoteRef = "Q-500",
        Title = "Quote5 Title",
        CustomerId = Customer1.Id
    };

    public static void SeedDatabase(BizTrakDbContext context)
    {
        var customers = new List<Customer>
            {
                Customer1, Customer2
            };

        var quotes = new List<Quote>
            {
                Quote1, Quote2, Quote3, Quote4, Quote5
            };

        var tasks = new List<QuoteTask> {
            Task1, Task2
        };

        var materials = new List<QuoteMaterial> {
            Material1, Material2
        };

        context.Customers.AddRange(customers);
        context.Quotes.AddRange(quotes);
        context.QuoteTasks.AddRange(tasks);
        context.QuoteMaterials.AddRange(materials);
        context.SaveChanges();
    }
}
