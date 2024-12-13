using System;
using BizTrak.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BizTrak.Data;

public class JobRepository(BizTrakDbContext BizTrakDbContext) : IJobRepository
{
    private readonly BizTrakDbContext BizTrakDbContext = BizTrakDbContext;

    public async Task<Job> AddAsync(Job newJob)
    {
        if (newJob == null)
            throw new ArgumentNullException(nameof(newJob), "Job cannot be null");


        await BizTrakDbContext.Jobs.AddAsync(newJob);
        return newJob;
    }

    public async Task<Job?> GetJobAsync(int id)
    {
        return await BizTrakDbContext.Jobs.FindAsync(id);
    }

    public async Task<Job?> GetJobByQuoteIdAsync(int quoteId)
    {
        return await BizTrakDbContext.Jobs.Where(j => j.QuoteId == quoteId).FirstOrDefaultAsync();
    }

    public bool JobExists(int id)
    {
        return BizTrakDbContext.Jobs.Any(q => q.Id == id);
    }

    public async Task<IEnumerable<Job>> ListScheduledAsync(DateTime start, DateTime end)
    {
        return await BizTrakDbContext.Jobs
            .AsNoTracking()
            .Where(j => j.Start < end && j.End > start && j.IsScheduled == true)
            .ToListAsync();
    }

    public async Task<IEnumerable<Job>> ListUnscheduledAsync()
    {
        return await BizTrakDbContext.Jobs
            .AsNoTracking()
            .Where(j => j.IsScheduled == false)
            .ToListAsync();
    }
}
