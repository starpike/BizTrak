using FinanceApp.Api;
using FinanceApp.Data;
using FinanceApp.DTO;
using FinanceApp.Domain;
using FinanceApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FinanceApp.UnitTests.Api.Controllers
{
    public class QuotesControllerTests
    {
        private readonly Mock<ILogger<QuotesController>> _mockLogger;
        private readonly Mock<IQuoteService> _mockQuoteService;
        private readonly Mock<IQuoteRepository> _mockQuoteRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly QuotesController _controller;

        public QuotesControllerTests()
        {
            _mockLogger = new Mock<ILogger<QuotesController>>();
            _mockQuoteService = new Mock<IQuoteService>();
            _mockQuoteRepository = new Mock<IQuoteRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new QuotesController(_mockLogger.Object, _mockQuoteService.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Should_Get_List_Of_All_Quotes()
        {
            // Arrange
            var mockQuotes = new List<Quote> { new Quote() { CustomerId = 0, Title = "" }, new Quote() { CustomerId = 0, Title = "" } };
            _mockQuoteRepository.Setup(repo => repo.ListQuotesAsync()).ReturnsAsync(mockQuotes);

            // Act
            var result = await _controller.ListQuotes(null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnQuotes = Assert.IsType<List<Quote>>(okResult.Value);
            Assert.Equal(mockQuotes.Count, returnQuotes.Count);
        }

        [Fact]
        public async Task Should_Get_Quote_By_Id()
        {
            // Arrange
            var mockQuote = new Quote { Id = 1, CustomerId = 0, Title = "" };
            _mockQuoteRepository.Setup(repo => repo.GetQuoteAsync(1, false)).ReturnsAsync(mockQuote);

            // Act
            var result = await _controller.GetQuoteById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnQuote = Assert.IsType<Quote>(okResult.Value);
            Assert.Equal(mockQuote.Id, returnQuote.Id);
        }

        [Fact]
        public async Task Should_Return_Quotes_With_Paging_Details()
        {
            // Arrange
            var mockPagedQuotes = new PagedQuotes
            {
                Quotes = new List<Quote> { new Quote() { CustomerId = 0, Title = "" }, new Quote() { CustomerId = 0, Title = "" } },
                Total = 2,
                PageSize = 10,
                Page = 1
            };

            _mockQuoteRepository.Setup(repo => repo.PagedQuotes(1, 10, null))
                                .ReturnsAsync(mockPagedQuotes);

            // Act
            var result = await _controller.ListQuotes(null, 1, 10, "");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPagedQuotes = Assert.IsType<PagedQuotes>(okResult.Value);
            Assert.Equal(mockPagedQuotes.Quotes, returnedPagedQuotes.Quotes);
            Assert.Equal(mockPagedQuotes.Total, returnedPagedQuotes.Total);
            Assert.Equal(1, returnedPagedQuotes.Page);
            Assert.Equal(10, returnedPagedQuotes.PageSize);
        }

        [Fact]
        public async Task Should_Create_And_Return_Quote()
        {
            // Arrange
            var mockQuoteDto = new QuoteDTO() { Title = ""};
            var mockCreatedQuote = new QuoteDTO { Id = 1, CustomerId = 0, Title = "" };
            _mockQuoteService.Setup(service => service.CreateQuoteAsync(mockQuoteDto))
                             .ReturnsAsync(mockCreatedQuote);

            // Act
            var result = await _controller.CreateQuote(mockQuoteDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnQuote = Assert.IsType<Quote>(createdResult.Value);
            Assert.Equal(mockCreatedQuote.Id, returnQuote.Id);
        }

        [Fact]
        public async Task Create_Quote_Should_Return_BadRequest_On_Validation_Exception()
        {
            // Arrange
            var mockQuoteDto = new QuoteDTO() { Title = "" };
            var mockException = new ValidationException(new List<ValidationError>());
            _mockQuoteService.Setup(service => service.CreateQuoteAsync(mockQuoteDto))
                             .ThrowsAsync(mockException);

            // Act
            var result = await _controller.CreateQuote(mockQuoteDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(mockException.Errors, badRequestResult.Value);
        }

        [Fact]
        public async Task Create_Quote_Should_Return_InternalServerError_On_Exception()
        {
            // Arrange
            var mockQuoteDto = new QuoteDTO() { Title = "" };
            _mockQuoteService.Setup(service => service.CreateQuoteAsync(mockQuoteDto))
                             .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.CreateQuote(mockQuoteDto);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerErrorResult.StatusCode);
            Assert.Equal("Internal server error", internalServerErrorResult.Value);
        }
    }
}
