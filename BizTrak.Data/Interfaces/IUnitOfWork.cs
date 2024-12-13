namespace BizTrak.Data;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    IQuoteRepository Quotes { get; }
    ICustomerRepository Customers { get; }
    IJobRepository Jobs { get; }
    IUserRepository Users { get; }

}
