using System;
using FinanceApp.Domain.Entities;

namespace FinanceApp.Data;

public interface IJobRepository
{
    Task<Job> AddAsync(Job newJob);
    Task<IEnumerable<Job>> ListScheduledAsync(DateTime start, DateTime end);
    Task<IEnumerable<Job>> ListUnscheduledAsync();
    Task<Job?> GetJobAsync(int id);
    bool JobExists(int id);
    Task<Job?> GetJobByQuoteIdAsync(int quoteId);
}
