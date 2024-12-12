using System;
using FinanceApp.Application.Exceptions;
using FinanceApp.Application.Validation;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Api.Middleware;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context); // Continue with the next middleware in the pipeline
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An exception occurred during the request.");

        var statusCode = exception switch
        {
            DbUpdateConcurrencyException => StatusCodes.Status409Conflict,
            DbUpdateException => StatusCodes.Status500InternalServerError,
            NotFoundException => StatusCodes.Status404NotFound,
            ValidationException => StatusCodes.Status400BadRequest,
            InvalidStateChangeException => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };

        var response = new
        {
            StatusCode = statusCode,
            Message = exception switch
            {
                DbUpdateConcurrencyException => "A concurrency conflict occurred while saving data.",
                DbUpdateException => "A database error occurred. Please try again later.",
                NotFoundException notFoundEx => notFoundEx.Message,
                ValidationException validationEx => validationEx.Message,
                InvalidStateChangeException invalidStateChangeException => invalidStateChangeException.Message,
                _ => "An unexpected error occurred. Please try again later."
            }
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        return context.Response.WriteAsJsonAsync(response);
    }
}