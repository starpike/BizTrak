namespace FinanceApp.Data;

using FinanceApp.Domain;

public interface ICustomerRepository {
    Task<Customer> GetCustomerAsync(int id);
    Task<IEnumerable<Customer>> Search(string search);
    Task CreateAsync(Customer customer);
    Task<PagedCustomers> PagedCustomers(int page, int pageSize, string search);
     void UpdateAsync(Customer customer, Customer updatedCustomer);
}