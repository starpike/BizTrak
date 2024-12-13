using System;
using BizTrak.Domain.Entities;

namespace BizTrak.Data;

public interface IUserRepository
{
    Task<User?> GetUserAsync(string username);
    Task<User> AddAsync(User user);
}
