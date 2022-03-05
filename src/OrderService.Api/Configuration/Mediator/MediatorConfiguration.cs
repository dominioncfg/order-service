using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Common.Behaviours;
using OrderService.Application.Common.Configuration;

namespace OrderService.Api.Configuration;

internal static class MediatorConfiguration
{
    public static IServiceCollection AddCustomMediatR(this IServiceCollection services)
    {
        services.AddMediatR(ApplicationConfiguration.GetApplicationAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        return services;
    }
}
