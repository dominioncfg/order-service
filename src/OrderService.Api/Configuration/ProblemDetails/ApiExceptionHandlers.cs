using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Common.Exceptions;
using System;

namespace OrderService.Api.Configuration
{
    internal static class ApiExceptionHandlers
    {
        private static string UnhandledExceptionTitle => "Whoops. Something went wrong";
        private static string ValidationExceptionTitle => "One or more validation failures have occurred.";

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

        public static ProblemDetails BadRequestExceptionHandler(BadRequestApplicatonException ex)
        {
            return new ValidationProblemDetails()
            {
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest,
                Title = ValidationExceptionTitle,
            };
        }
    }
}
