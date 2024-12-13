using BizTrak.Application.Exceptions;
using BizTrak.DTO;

namespace BizTrak.Application.Validation;

public interface IJobValidationService {
    Task<ValidationResult> ValidateAsync(JobDTO jobDTO);
}

public class JobValidationService : IJobValidationService
{
    public Task<ValidationResult> ValidateAsync(JobDTO job)
    {
       var errors = new List<ValidationError>();

        if (job.QuoteId <= 0)
        {
            errors.Add(new ValidationError("QuoteId", "QuoteId is missing"));
        }

        if (string.IsNullOrWhiteSpace(job.Title))
        {
            errors.Add(new ValidationError("Title", "Job title is empty"));
        }
       
        if (job.Start == job.End) {
            errors.Add(new ValidationError("Start", "Job start cannot be the same as job end"));
        }

        if (job.Start > job.End) {
            errors.Add(new ValidationError("Start", "Job end cannot be before job start"));
        }

        return Task.FromResult(new ValidationResult(errors.Count == 0, errors));    }
}
