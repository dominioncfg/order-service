using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Common.Exceptions;
using System;

namespace OrderService.Api.Configuration;

internal static class ProblemDetailsConfiguration
{
    private static string ValidationErrorMessage => "Please refer to the errors property for additional details.";
    private static string ErrorJsonContentType => "application/problem+json";
    private static string ErrorXmlContentType => "application/problem+xml";


    public static IServiceCollection ConfigureProblemDetails(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = ProblemDetailsApiBehaviorConfiguration;
        });
        services.AddProblemDetails(opts =>
        {
            opts.IncludeExceptionDetails = (_, __) => false;
            opts.Map<ValidationException>(ApiExceptionHandlers.FluentValidationExceptionHandler);
            opts.Map<BadRequestApplicationException>(ApiExceptionHandlers.BadRequestExceptionHandler);
            opts.Map<EntityNotFoundApplicationException>(ApiExceptionHandlers.EntityNotFoundExceptionHandler);
            opts.Map<Exception>(ex => ApiExceptionHandlers.UnhandledExceptionHandler(ex));
        });
        return services;
    }

    public static IApplicationBuilder UseCustomProblemDetails(this IApplicationBuilder app)
    {
        app.UseProblemDetails();
        return app;
    }

    public static IActionResult ProblemDetailsApiBehaviorConfiguration(ActionContext context)
    {
        var problemDetails = new ValidationProblemDetails(context.ModelState)
        {
            Instance = context.HttpContext.Request.Path,
            Status = StatusCodes.Status400BadRequest,
            Type = $"https://httpstatuses.com/400",
            Detail = ValidationErrorMessage
        };
        return new BadRequestObjectResult(problemDetails)
        {
            ContentTypes = {
                    ErrorJsonContentType,
                    ErrorXmlContentType
                    }
        };
    }
}
