using System;
using BizTrak.Domain.Entities;
using BizTrak.DTO;

namespace BizTrak.Domain.Extensions;

public static class CustomerExtensions
{
    public static CustomerDTO ToDTO(this Customer customer) {
        return new CustomerDTO {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            Address = customer.Address
        };
    }

    public static List<CustomerDTO> ToDTOList(this IEnumerable<Customer> customers)
    {
        return customers.Select(c => c.ToDTO()).ToList();
    }
}
