using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Domain.Entities;

public class User
{
    [Key]
    public int Id { get; set; }
    [Required]
    public required string Username { get; set; }
    [Required]
    public required string PasswordHash { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = [];
}
