namespace BizTrak.Data;

using BizTrak.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class QuoteTaskRepository(BizTrakDbContext BizTrakDbContext) : IQuoteTaskRepository
{
    private readonly BizTrakDbContext _BizTrakDbContext = BizTrakDbContext;

    public async Task<IEnumerable<QuoteTask>> ListTasksAsync()
    {
        return await _BizTrakDbContext.QuoteTasks.ToListAsync();
    }

    public async Task<QuoteTask> GetTaskAsync(int id)
    {
        var task =  await _BizTrakDbContext.QuoteTasks.FindAsync(id);
        return task!;
    }
}