using Microsoft.EntityFrameworkCore.Storage;

namespace FinanceApp.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly FinanceAppDbContext _context;
    private IDbContextTransaction? _transaction;
    private IQuoteRepository? _quoteRepository;
    private ICustomerRepository? _customersRepository;
    private IJobRepository? _jobsRepository;
    private IUserRepository? _usersRepository;
    public IQuoteRepository Quotes => _quoteRepository ??= new QuoteRepository(_context);
    public ICustomerRepository Customers => _customersRepository ??= new CustomerRepository(_context);
    public IJobRepository Jobs => _jobsRepository ??= new JobRepository(_context);
    public IUserRepository Users => _usersRepository ??= new UserRepository(_context);

    public UnitOfWork(FinanceAppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _context?.Dispose();
        _transaction?.Dispose();
    }
}
