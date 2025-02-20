using BizTrak.Application.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BizTrak.Api.Extensions;

public static class ModelStateExtensions
{
    public static void AddValidationErrors(this ModelStateDictionary modelState, IEnumerable<ValidationError> errors)
    {
        foreach (var error in errors)
        {
            modelState.AddModelError(error.Field, error.Message);
        }
    }
}
