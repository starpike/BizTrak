using System;
using FinanceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Data;

public class UserRepository(FinanceAppDbContext context) : IUserRepository
{
    private readonly FinanceAppDbContext context = context;

    public async Task<User?> GetUserAsync(string username)
    {
        return await context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role) 
            .SingleOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User> AddAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user), "User cannot be null");


        await context.Users.AddAsync(user);
        return user;
    }
}
