using Microsoft.AspNetCore.Http;
using OrderService.Application.Common.Exceptions;
using OrderService.Seedwork.Domain;

namespace OrderService.Api.Configuration;

internal static class ApiExceptionHandlers
{
    private static string UnhandledExceptionTitle => "Whoops. Something went wrong";
    private static string ValidationExceptionTitle => "One or more validation failures have occurred.";
    private static string BadRequestExceptionTitle => "Looks like there is something wrong with your request.";
    private static string NotFoundExceptionTitle => "Entity Not Found";
    private static string DomainExceptionTitle => "Fail to do the requested operation.";


    public static ProblemDetails UnhandledExceptionHandler(Exception ex)
    {
        return new ProblemDetails
        {
            Detail = ex.Message,
            Status = StatusCodes.Status500InternalServerError,
            Title = UnhandledExceptionTitle
        };
    }

    public static ProblemDetails FluentValidationExceptionHandler(ValidationException ex)
    {
        return new ValidationProblemDetails(ex.Errors)
        {
            Detail = ex.Message,
            Status = StatusCodes.Status400BadRequest,
            Title = ValidationExceptionTitle,
        };
    }

    public static ProblemDetails BadRequestExceptionHandler(BadRequestApplicationException ex)
    {
        return new ValidationProblemDetails()
        {
            Detail = ex.Message,
            Status = StatusCodes.Status400BadRequest,
            Title = BadRequestExceptionTitle,
        };
    }

    public static ProblemDetails EntityNotFoundExceptionHandler(EntityNotFoundApplicationException ex)
    {
        return new ValidationProblemDetails()
        {
            Detail = ex.Message,
            Status = StatusCodes.Status404NotFound,
            Title = NotFoundExceptionTitle,
        };
    }

    public static ProblemDetails BaseDomainExceptionHandler(DomainException ex)
    {
        return new ValidationProblemDetails()
        {
            Detail = ex.Message,
            Status = StatusCodes.Status400BadRequest,
            Title = DomainExceptionTitle,
        };
    }
}
