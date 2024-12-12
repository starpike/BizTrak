using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceApp.Domain.Entities;

public class Role
{
    [Key]
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = [];

    public enum RoleType {
        User = 1,
        Admin = 2,
    }
}
