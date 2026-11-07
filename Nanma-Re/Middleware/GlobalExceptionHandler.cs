using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nanma.Domain.Exceptions;

namespace Nanma_Re.Middleware;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ProblemDetails problemDetails;
        int statusCode;

        if (exception is ValidationException validationException)
        {
            (problemDetails, statusCode) = HandleValidation(validationException);
        }
        else if (exception is NotFoundException notFound)
        {
            (problemDetails, statusCode) = HandleKnownAppException(notFound, StatusCodes.Status404NotFound, "Resource not found");
        }
        else if (exception is ConflictException conflict)
        {
            (problemDetails, statusCode) = HandleKnownAppException(conflict, StatusCodes.Status409Conflict, "Conflict");
        }
        else if (exception is ForbiddenException forbidden)
        {
            (problemDetails, statusCode) = HandleKnownAppException(forbidden, StatusCodes.Status403Forbidden, "Forbidden");
        }
        else if (exception is UnauthorizedException unauthorized)
        {
            (problemDetails, statusCode) = HandleKnownAppException(unauthorized, StatusCodes.Status401Unauthorized, "Unauthorized");
        }
        else
        {
            (problemDetails, statusCode) = HandleUnexpectedException(httpContext, exception);
        }

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }

    private static (ProblemDetails problemDetails, int statusCode) HandleValidation(ValidationException exception)
    {
        var problem = new ValidationProblemDetails(
            exception.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()))
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation failed",
            Type = "https://nanma.dev/errors/validation"
        };

        problem.Extensions["code"] = "validation_failed";
        return (problem, StatusCodes.Status400BadRequest);
    }

    private static (ProblemDetails problemDetails, int statusCode) HandleKnownAppException(AppException exception, int statusCode, string title)
    {
        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message,
            Type = $"https://nanma.dev/errors/{exception.ErrorCode}"
        };

        problem.Extensions["code"] = exception.ErrorCode;
        return (problem, statusCode);
    }

    private (ProblemDetails problemDetails, int statusCode) HandleUnexpectedException(HttpContext httpContext, Exception exception)
    {
        logger.LogError(exception, "Unhandled exception for {Method} {Path}", httpContext.Request.Method, httpContext.Request.Path);

        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An unexpected error occurred",
            Type = "https://nanma.dev/errors/internal_server_error"
        };

        problem.Extensions["code"] = "internal_server_error";
        return (problem, StatusCodes.Status500InternalServerError);
    }
}
