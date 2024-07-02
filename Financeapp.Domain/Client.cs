using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Domain;

public class Client
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? AddressLine3 { get; set; }
    public string Postcode { get; set; }

}