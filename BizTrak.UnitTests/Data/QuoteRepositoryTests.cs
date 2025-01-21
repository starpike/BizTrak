using System.Linq.Expressions;
using BizTrak.Data;
using BizTrak.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BizTrak.UnitTests.Data;

public class QuoteRepositoryTests : IDisposable
{
    private readonly DbContextOptions<BizTrakDbContext> _options;
    private readonly BizTrakDbContext _seedContext;

    public QuoteRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<BizTrakDbContext>()
           .UseInMemoryDatabase(databaseName: $"BizTrakTestDb_{Guid.NewGuid()}")
           .Options;

        _seedContext = new BizTrakDbContext(_options);
        _seedContext.Database.EnsureCreated();
        InMemoryData.SeedDatabase(_seedContext);
        _seedContext.SaveChangesAsync();
    }

    [Fact]
    public async void ListQuotesAsync_Should_Return_AllQuotes()
    {
        using (var context = new BizTrakDbContext(_options))
        {
            var repository = new QuoteRepository(context);
            var result = await repository.ListQuotesAsync();

            Assert.NotNull(result);

            var quotes = result.ToList();
            Assert.Equal(5, quotes.Count);

            Assert.Equal("Test Customer 1", quotes[0].Customer.Name);
            Assert.Equal("Test Customer 2", quotes[1].Customer.Name);
        }
    }

    [Fact]
    public async Task PagedQuotes_Returns_Correct_Page_And_Total()
    {
        // Arrange
        using (var context = new BizTrakDbContext(_options))
        {
            var repository = new QuoteRepository(context);

            // Act
            var page = 1;
            var pageSize = 2;
            Expression<Func<Quote, bool>> filter = q => true; // No filtering
            var result = await repository.PagedQuotes(page, pageSize, filter);

            // Assert
            Assert.Equal(5, result.Total); // Total quotes
            Assert.Equal(2, result.Quotes.Count()); // Page size
            Assert.Equal(5, result.Quotes.First().Id); // Ordered descending by Id
        }
    }

    [Fact]
    public async Task PagedQuotes_Returns_Empty_For_No_Matching_Filter()
    {
        // Arrange
        using (var context = new BizTrakDbContext(_options))
        {
            var repository = new QuoteRepository(context);

            // Act
            Expression<Func<Quote, bool>> filter = q => q.Customer.Name == "Nonexistent Customer";
            var result = await repository.PagedQuotes(1, 5, filter);

            // Assert
            Assert.Equal(0, result.Total); // No matching quotes
            Assert.Empty(result.Quotes); // No quotes in the result
        }
    }

    [Fact]
    public async Task GetQuoteAsync_Should_Get_Quote_By_Id()
    {
        using (var context = new BizTrakDbContext(_options))
        {

            var repository = new QuoteRepository(context);

            var quote = await repository.GetQuoteAsync(1);

            Assert.Equal(1, quote.Id);
            Assert.Equal("Quote1 Title", quote.Title);
        }
    }

    [Fact]
    public async Task GetQuoteAsync_Should_Get_Tasks_And_Materials_When_IncludeRelated_True()
    {
        using (var context = new BizTrakDbContext(_options))
        {

            var repository = new QuoteRepository(context);

            var quote = await repository.GetQuoteAsync(1, true);

            Assert.Equal(1, quote.Id);
            Assert.Equal(2, quote.Tasks.Count());
            Assert.Equal(2, quote.Materials.Count());
        }
    }

    [Fact]
    public async Task GetQuoteAsync_Should_Not_Get_Tasks_And_Materials_When_IncludeRelated_False()
    {
        using (var context = new BizTrakDbContext(_options))
        {

            var repository = new QuoteRepository(context);

            var quote = await repository.GetQuoteAsync(1);

            Assert.Equal(1, quote.Id);
            Assert.Empty(quote.Tasks);
            Assert.Empty(quote.Materials);
        }
    }

    [Fact]
    public async Task AddAsync_WhenQuoteIsNull_ShouldThrowArgumentNullException()
    {
        using var context = new BizTrakDbContext(_options);
        var repository = new QuoteRepository(context);

        await Assert.ThrowsAsync<ArgumentNullException>(() => repository.AddAsync(null!));
    }

    [Fact]
    public async Task AddAsync_WhenQuoteIsValid_ShouldAddQuoteAndReturnIt()
    {
        using var context = new BizTrakDbContext(_options);
        var repository = new QuoteRepository(context);

        var quote = new Quote
        {
            Id = 123,
            QuoteRef = "Q-123",
            Title = "Test Quote",
            CustomerId = 1
        };

        var result = await repository.AddAsync(quote);

        Assert.NotNull(result);

        Assert.Equal(quote, result);

        // The quote should be in the context's ChangeTracker,
        // but not saved to the DB yet.
        Assert.Single(context.Quotes.Local);
        Assert.Same(quote, context.Quotes.Local.First());
    }

    [Fact]
    public async Task UpdateAsync_WhenQuoteIsNull_ShouldThrowArgumentNullException()
    {
        using var context = new BizTrakDbContext(_options);
        var repository = new QuoteRepository(context);

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
        {
            return Task.Run(() => repository.UpdateAsync(null!, InMemoryData.Quote1));
        });

        Assert.Contains("Quote cannot be null", ex.Message);
    }

    [Fact]
    public async Task UpdateAsync_WhenValid_ShouldSetValuesFromUpdatedData()
    {
        // Arrange
        using var context = new BizTrakDbContext(_options);
        var repository = new QuoteRepository(context);

        var originalQuote = new Quote
        {
            QuoteRef = "Q-Original1",
            Title = "Original Title",
            CustomerId = 1
        };

        context.Quotes.Add(originalQuote);
        await context.SaveChangesAsync();

        var quoteToUpdate = await context.Quotes.FirstOrDefaultAsync(q => q.QuoteRef == "Q-Original1");

        var updatedData = new Quote
        {
            Id = quoteToUpdate.Id,
            QuoteRef = "Q-Updated1",
            Title = "Updated Title",
            CustomerId = 2
        };

        repository.UpdateAsync(quoteToUpdate!, updatedData);
        await context.SaveChangesAsync();

        var updatedQuote = await context.Quotes.FirstOrDefaultAsync(q => q.Id == quoteToUpdate!.Id);
        Assert.NotNull(updatedQuote);
        Assert.Equal("Q-Updated1", updatedQuote!.QuoteRef);
        Assert.Equal("Updated Title", updatedQuote.Title);
        Assert.Equal(2, updatedQuote.CustomerId);

    }

    public void Dispose()
    {
        _seedContext.Database.EnsureDeleted();
        _seedContext.Dispose();
    }
}
