namespace FinanceApp.Data;

using FinanceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class QuoteTaskRepository(FinanceAppDbContext financeAppDbContext) : IQuoteTaskRepository
{
    private readonly FinanceAppDbContext _financeAppDbContext = financeAppDbContext;

    public async Task<IEnumerable<QuoteTask>> ListTasksAsync()
    {
        return await _financeAppDbContext.QuoteTasks.ToListAsync();
    }

    public async Task<QuoteTask> GetTaskAsync(int id)
    {
        var task =  await _financeAppDbContext.QuoteTasks.FindAsync(id);
        return task!;
    }
}