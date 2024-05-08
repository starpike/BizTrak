namespace FinanceApp.Data;

using System.Threading.Tasks;
using FinanceApp.Domain;


public interface IJobRepository {
    public Task<IEnumerable<Job>> AllJobsAsync();
    public Task<Job> GetJobAsync(int id);

}