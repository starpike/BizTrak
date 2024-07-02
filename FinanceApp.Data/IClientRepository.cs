namespace FinanceApp.Data;

using FinanceApp.Domain;

public interface IClientRepository {
    public Task<Client> GetClientAsync(int id);
    public Task<IEnumerable<Client>> Search(string search);
}