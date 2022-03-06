using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Common.Configuration;
using System.Reflection;

namespace OrderService.Api.Configuration;

internal static class AutomapperConfiguration
{
    public static IServiceCollection AddCustomAutomapper(this IServiceCollection services)
    {
        var assamblies = new Assembly[]
        {
            Assembly.GetExecutingAssembly(), 
            ApplicationConfiguration.GetApplicationAssembly(),
        };
        services.AddAutoMapper(assamblies);
        return services;
    }
}
