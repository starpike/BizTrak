namespace BizTrak.Data;

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BizTrak.Domain.Entities;

public class CustomerRepository : ICustomerRepository
{
    private readonly BizTrakDbContext _BizTrakDbContext;

    public CustomerRepository(BizTrakDbContext BizTrakDbContext)
    {
        _BizTrakDbContext = BizTrakDbContext;
    }

    public async Task<Customer> GetCustomerAsync(int id)
    {
        var customer = await _BizTrakDbContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
        return customer!;
    }

    public async Task AddAsync(Customer customer)
    {
        if (customer == null)
            throw new ArgumentNullException(nameof(customer), "Customer cannot be null");


        await _BizTrakDbContext.Customers.AddAsync(customer);
    }

    public void UpdateAsync(Customer customer, Customer updatedCustomer)
    {
        if (customer == null)
            throw new ArgumentNullException(nameof(customer), "Customer cannot be null");

        _BizTrakDbContext.Entry(customer).CurrentValues.SetValues(updatedCustomer);
    }

    public async Task<IEnumerable<Customer>> Search(string search)
    {
        var query = _BizTrakDbContext.Customers.AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(q => q.Name.ToLower().Contains(search.ToLower()) ||
                q.Address.ToLower().Contains(search.ToLower()));
        }

        var customers = await query
            .OrderByDescending(q => q.Name)
            .Take(10)
            .ToListAsync();

        return customers;
    }

    public async Task<PagedCustomers> PagedCustomers(int page, int pageSize, string search)
    {
        var query = _BizTrakDbContext.Customers
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(q => q.Name.Contains(search) || q.Address.Contains(search));
        }

        var pagedCustomers = new PagedCustomers
        {
            Total = await query.CountAsync(),
            Customers = await query
                .OrderByDescending(q => q.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(),
            Page = page,
            PageSize = pageSize
        };

        return pagedCustomers;
    }
}
