using System;

namespace BizTrak.Domain.Entities;

public class Customer
{
    public int Id { get; set;}
    public required string Name { get; set;}
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public required string Address { get; set; }
}
