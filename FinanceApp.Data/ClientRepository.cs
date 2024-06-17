namespace FinanceApp.Data;

using System.Threading.Tasks;
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
}
