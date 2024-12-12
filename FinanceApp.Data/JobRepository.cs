using System;
using FinanceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Data;

public class JobRepository(FinanceAppDbContext financeAppDbContext) : IJobRepository
{
    private readonly FinanceAppDbContext financeAppDbContext = financeAppDbContext;

    public async Task<Job> AddAsync(Job newJob)
    {
        if (newJob == null)
            throw new ArgumentNullException(nameof(newJob), "Job cannot be null");


        await financeAppDbContext.Jobs.AddAsync(newJob);
        return newJob;
    }

    public async Task<Job?> GetJobAsync(int id)
    {
        return await financeAppDbContext.Jobs.FindAsync(id);
    }

    public async Task<Job?> GetJobByQuoteIdAsync(int quoteId)
    {
        return await financeAppDbContext.Jobs.Where(j => j.QuoteId == quoteId).FirstOrDefaultAsync();
    }

    public bool JobExists(int id)
    {
        return financeAppDbContext.Jobs.Any(q => q.Id == id);
    }

    public async Task<IEnumerable<Job>> ListScheduledAsync(DateTime start, DateTime end)
    {
        return await financeAppDbContext.Jobs
            .AsNoTracking()
            .Where(j => j.Start < end && j.End > start && j.IsScheduled == true)
            .ToListAsync();
    }

    public async Task<IEnumerable<Job>> ListUnscheduledAsync()
    {
        return await financeAppDbContext.Jobs
            .AsNoTracking()
            .Where(j => j.IsScheduled == false)
            .ToListAsync();
    }
}
