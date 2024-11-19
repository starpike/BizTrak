using System;

namespace FinanceApp.DTO;

public class CustomerDTO
{
    public int Id { get; set;}
    public required string Name { get; set;}
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public required string Address { get; set; }

    public static object Adapt<T>()
    {
        throw new NotImplementedException();
    }
}
