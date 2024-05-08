namespace FinanceApp.Data;

using FinanceApp.Domain;
using Microsoft.EntityFrameworkCore;

public class JobRepository(FinanceAppDbContext financeAppDbContext) : IJobRepository
{
    private readonly FinanceAppDbContext _financeAppDbContext = financeAppDbContext;

    public async Task<IEnumerable<Job>> AllJobsAsync()
    {
        return await _financeAppDbContext.Jobs.ToListAsync();
    }

    public async Task<Job> GetJobAsync(int id)
    {
        var job =  await _financeAppDbContext.Jobs.FindAsync(id);
        return job!;
    }
}