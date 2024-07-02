namespace FinanceApp.Data;

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinanceApp.Domain;

public class ClientRepository : IClientRepository
{
    private readonly FinanceAppDbContext _financeAppDbContext;

    public ClientRepository(FinanceAppDbContext financeAppDbContext)
    {
        _financeAppDbContext = financeAppDbContext;
    }

    public async Task<Client> GetClientAsync(int id)
    {
        var client = await _financeAppDbContext.Clients.FindAsync(id);
        return client;
    }

    public async Task<IEnumerable<Client>> Search(string search)
    {
        var query = _financeAppDbContext.Clients.AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(q => q.FirstName.ToLower().Contains(search.ToLower()) ||
                q.LastName.ToLower().Contains(search.ToLower()) ||
                q.AddressLine1.ToLower().Contains(search.ToLower()) ||
                q.Postcode.ToLower().Contains(search.ToLower()));
        }

        var clients = await query
            .OrderByDescending(q => q.LastName)
            .Take(10)
            .ToListAsync();

        return clients;
    }
}
