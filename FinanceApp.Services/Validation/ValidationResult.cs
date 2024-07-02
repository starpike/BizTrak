namespace FinanceApp.Services;

public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<ValidationError> Errors { get; set; }

    public ValidationResult(bool isValid, List<ValidationError> errors)
    {
        IsValid = isValid;
        Errors = errors;
    }
}