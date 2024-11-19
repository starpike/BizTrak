using FinanceApp.DTO;

namespace FinanceApp.Domain;

public class PagedCustomers {
    private IEnumerable<Customer> customers = new List<Customer>();
    public IEnumerable<Customer> Customers { get => customers; set => customers = value; }
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}