using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BizTrak.Data;
using BizTrak.Domain.Entities;
using BizTrak.DTO;
using Microsoft.IdentityModel.Tokens;

namespace BizTrak.Application.Services;

public class AuthenticationService(IUnitOfWork unitOfWork) : IAuthenticationService
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<User> RegisterUser(RegisterDTO registerDTO)
    {
        var user = new User
        {
            Username = registerDTO.Username,
            PasswordHash = HashPassword(registerDTO.Password)
        };

        user.UserRoles.Add(new UserRole { User = user, RoleId = (int)Role.RoleType.User });

        await unitOfWork.Users.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        return user;
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private static bool VerifyPassword(string enteredPassword, string storedPasswordHash)
    {
        return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPasswordHash);
    }

    public static string GenerateJwtToken(User user, JwtSettings jwtSettings)
    {
        var roles = user.UserRoles
            .Select(ur => ur.Role?.Name)
            .Where(roleName => !string.IsNullOrEmpty(roleName))
            .ToList();

        var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username)
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings.ValidAudience,
            audience: jwtSettings.ValidIssuer,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public interface IAuthenticationService
{
    Task<User> RegisterUser(RegisterDTO registerDTO);
}