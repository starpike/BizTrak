using System;

namespace FinanceApp.Application.Validation;

public class ValidationResult
{
    public bool IsValid { get; set; }
    public IEnumerable<ValidationError> Errors { get; set; }

    public ValidationResult(bool isValid, IEnumerable<ValidationError> errors)
    {
        IsValid = isValid;
        Errors = errors;
    }
}
