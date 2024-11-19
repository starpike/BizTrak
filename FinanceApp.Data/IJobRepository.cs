using System;
using FinanceApp.Domain;

namespace FinanceApp.Data;

public interface IJobRepository
{
    Task<Job> CreateJob(Job newJob);
    Task<IEnumerable<Job>> ListJobs(DateTime start, DateTime end);
}
