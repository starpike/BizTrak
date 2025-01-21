using System.Text.RegularExpressions;
using BizTrak.DTO;

namespace BizTrak.Application.Validation;

public interface ICustomerValidationsService
{
    ValidationResult Validate(CustomerDTO customer);
}

public class CustomerValidationService : ICustomerValidationsService
{
    public ValidationResult Validate(CustomerDTO customer)
    {
        var errors = new List<ValidationError>();

        if (customer == null)
        {
            errors.Add(new ValidationError("customerDTO", "Customer cannot be empty"));
            return new ValidationResult(false, errors);
        }

        if (string.IsNullOrWhiteSpace(customer.Name))
            errors.Add(new ValidationError("Name", "Customer name cannot be empty"));
        else if (customer.Name.Length > 100)
            errors.Add(new ValidationError("Name", "Customer name cannot be longer than 100 characters"));
        else if (!Regex.IsMatch(customer.Name, @"^[a-zA-Z\s]+$"))
            errors.Add(new ValidationError("Name", "Customer name can only contain letters and spaces"));

        if (string.IsNullOrWhiteSpace(customer.Address))
            errors.Add(new ValidationError("Address", "Customer address cannot be empty"));
        else if (customer.Address.Length > 250)
            errors.Add(new ValidationError("Address", "Customer address cannot be longer than 250 characters"));

        if (!string.IsNullOrWhiteSpace(customer.Email) &&  !Regex.IsMatch(customer.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            errors.Add(new ValidationError("Email", "Invalid email format"));

        if (!string.IsNullOrWhiteSpace(customer.Phone) && !Regex.IsMatch(customer.Phone, @"^\+?\d{10,15}$"))
            errors.Add(new ValidationError("Phone", "Invalid phone number format"));

        return new ValidationResult(errors.Count == 0, errors);
    }
}
