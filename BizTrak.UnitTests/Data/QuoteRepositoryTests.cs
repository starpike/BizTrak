using BizTrak.Data;
using BizTrak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BizTrak.UnitTests.Data;

public class QuoteRepositoryTests
{
    private readonly Mock<BizTrakDbContext> _mockContext;
    private readonly Mock<DbSet<Quote>> _mockSet;
    private readonly QuoteRepository _repository;

    public QuoteRepositoryTests()
    {
        _mockContext = new Mock<BizTrakDbContext>();
        _mockSet = new Mock<DbSet<Quote>>();
        _mockContext.Setup(c => c.Quotes).Returns(_mockSet.Object);
        _repository = new QuoteRepository(_mockContext.Object);
    }

    [Fact]
    public async Task Should_Return_All_Quotes()
    {
        var data = new List<Quote>
        {
            new Quote { Id = 1, QuoteRef = "Ref1", Title = "", CustomerId = 0 },
            new Quote { Id = 2, QuoteRef = "Ref2", Title = "", CustomerId = 0 }
        }.AsQueryable();

        _mockSet.As<IQueryable<Quote>>().Setup(m => m.Provider).Returns(data.Provider);
        _mockSet.As<IQueryable<Quote>>().Setup(m => m.Expression).Returns(data.Expression);
        _mockSet.As<IQueryable<Quote>>().Setup(m => m.ElementType).Returns(data.ElementType);
        _mockSet.As<IQueryable<Quote>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        var result = await _repository.ListQuotesAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Should_Return_Quote_By_Id()
    {
        var quote = new Quote { Id = 1, QuoteRef = "Ref1", Title = "", CustomerId = 0 };
        _mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync(quote);

        var result = await _repository.GetQuoteAsync(1);

        Assert.Equal(quote, result);
    }

    [Fact]
    public async Task Should_Create_Quote()
    {
        // Arrange
        var quote = new Quote { Id = 1, QuoteRef = "Ref1", Title = "", CustomerId = 0 };

        // Act
        await _repository.AddAsync(quote);

        // Assert
        _mockSet.Verify(m => m.AddAsync(quote, default), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public void Should_Update_Quote()
    {
        // Arrange
        var quote = new Quote { Id = 1, QuoteRef = "Ref1", Title = "", CustomerId = 0 };

        // Act
        _repository.UpdateAsync(quote, quote);

        // Assert
        _mockSet.Verify(m => m.Update(quote), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Should_Return_Paged_Quotes()
    {
        var data = new List<Quote>
        {
            new Quote { Id = 1, QuoteRef = "Ref1", Title = "", CustomerId = 0 },
            new Quote { Id = 2, QuoteRef = "Ref2", Title = "", CustomerId = 0 },
            new Quote { Id = 3, QuoteRef = "Ref3", Title = "", CustomerId = 0 }
        }.AsQueryable();

        _mockSet.As<IQueryable<Quote>>().Setup(m => m.Provider).Returns(data.Provider);
        _mockSet.As<IQueryable<Quote>>().Setup(m => m.Expression).Returns(data.Expression);
        _mockSet.As<IQueryable<Quote>>().Setup(m => m.ElementType).Returns(data.ElementType);
        _mockSet.As<IQueryable<Quote>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        var result = await _repository.PagedQuotes(1, 2, null);

        Assert.Equal(3, result.Total);
        Assert.Equal(2, result.Quotes.Count());
    }
}
