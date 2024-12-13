namespace BizTrak.Data;

using System.Threading.Tasks;
using BizTrak.Domain.Entities;

public interface IQuoteTaskRepository {
    public Task<IEnumerable<QuoteTask>> ListTasksAsync();
    public Task<QuoteTask> GetTaskAsync(int id);

}