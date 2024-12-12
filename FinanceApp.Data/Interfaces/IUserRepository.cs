using System;
using FinanceApp.Domain.Entities;

namespace FinanceApp.Data;

public interface IUserRepository
{
    Task<User?> GetUserAsync(string username);
    Task<User> AddAsync(User user);
}
