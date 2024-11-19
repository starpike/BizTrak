using System;
using FinanceApp.Domain;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Data;

public class JobRepository(FinanceAppDbContext financeAppDbContext) : IJobRepository
{
    private readonly FinanceAppDbContext financeAppDbContext = financeAppDbContext;

    public async Task<Job> CreateJob(Job newJob)
    {
        if (newJob == null)
            throw new ArgumentNullException(nameof(newJob), "Job cannot be null");


        await financeAppDbContext.Jobs.AddAsync(newJob);
        return newJob;    
    }

    public async Task<IEnumerable<Job>> ListJobs(DateTime start, DateTime end)
    {
        return await financeAppDbContext.Jobs
            .AsNoTracking()
            .Where(j => j.Start < end && j.End > start)
            .ToListAsync();
    }
}
