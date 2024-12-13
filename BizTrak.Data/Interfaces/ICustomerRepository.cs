namespace BizTrak.Data;

using BizTrak.Domain.Entities;

public interface ICustomerRepository {
    Task<Customer> GetCustomerAsync(int id);
    Task<IEnumerable<Customer>> Search(string search);
    Task AddAsync(Customer customer);
    Task<PagedCustomers> PagedCustomers(int page, int pageSize, string search);
     void UpdateAsync(Customer customer, Customer updatedCustomer);
}