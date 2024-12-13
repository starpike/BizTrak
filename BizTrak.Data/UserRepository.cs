using System;
using BizTrak.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BizTrak.Data;

public class UserRepository(BizTrakDbContext context) : IUserRepository
{
    private readonly BizTrakDbContext context = context;

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
