namespace FinanceApp.Data;

using System.Threading.Tasks;
using FinanceApp.Domain;

public interface IQuoteTaskRepository {
    public Task<IEnumerable<QuoteTask>> ListTasksAsync();
    public Task<QuoteTask> GetTaskAsync(int id);

}