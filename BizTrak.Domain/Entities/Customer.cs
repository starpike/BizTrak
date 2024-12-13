using System;

namespace BizTrak.Domain.Entities;

public class Customer
{
    public int Id { get; set;}
    public required string Name { get; set;}
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public required string Address { get; set; }
}
