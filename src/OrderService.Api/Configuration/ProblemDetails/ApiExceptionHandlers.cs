using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Common.Exceptions;
using System;

namespace OrderService.Api.Configuration;

internal static class ApiExceptionHandlers
{
    private static string UnhandledExceptionTitle => "Whoops. Something went wrong";
    private static string ValidationExceptionTitle => "One or more validation failures have occurred.";
    private static string BadRequestExceptionTitle => "Looks like there is something wrong with your request.";
    private static string NotFoundExceptionTitle => "Entity Not Found";


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
}
