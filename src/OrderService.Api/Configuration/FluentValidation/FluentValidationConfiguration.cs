using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Common.Configuration;

namespace OrderService.Api.Configuration
{
    internal static class FluentValidationConfiguration
    {
        public static IServiceCollection AddCustomFluentValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(ApplicationConfiguration.GetApplicationAssembly());
            return services;
        }
    }
}
