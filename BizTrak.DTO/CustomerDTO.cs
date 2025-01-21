using System;

namespace BizTrak.DTO;

public class CustomerDTO
{
    public int Id { get; set;}
    public required string Name { get; set;}
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public required string Address { get; set; }

    public static object Adapt<T>()
    {
        throw new NotImplementedException();
    }
}
