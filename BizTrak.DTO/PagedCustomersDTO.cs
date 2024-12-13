using System;

namespace BizTrak.DTO;

public class PagedCustomersDTO
{
    private IEnumerable<CustomerDTO> customers = new List<CustomerDTO>();
    public IEnumerable<CustomerDTO> Customers { get => customers; set => customers = value; }
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
