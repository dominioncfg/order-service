using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace OrderService.Api.Configuration
{
    internal static class AutomapperConfiguration
    {
        public static IServiceCollection AddCustomAutomapper(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
