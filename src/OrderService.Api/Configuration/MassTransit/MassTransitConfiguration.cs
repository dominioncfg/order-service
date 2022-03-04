using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OrderService.Api.Configuration
{
    internal static class MassTransitConfiguration
    {
        public static IServiceCollection AddCustomMassTransit(this IServiceCollection services, IWebHostEnvironment env)
        {
            //Dont Use this for Testing. In testing we use InMemoryTransport
            if (env.IsEnvironment("Test"))
                return services;

            services.AddMassTransit(cfg =>
            {
                
                cfg.UsingRabbitMq( (x,y) =>
                {
                    y.Host("rabbitmq", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                });
            });

            services.AddMassTransitHostedService();

            return services;
        }

    }
}
