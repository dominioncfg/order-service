using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OrderService.Api.Configuration;

internal static class MassTransitConfiguration
{
    public static IServiceCollection AddCustomMassTransit(this IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration)
    {
        //Dont Use this for Testing. In testing we use InMemoryTransport
        if (env.IsEnvironment("Test"))
            return services;


        var rabbitMqOptions = new RabbitMqOptions();
        configuration.GetSection(RabbitMqOptions.Section).Bind(rabbitMqOptions);
        services.AddMassTransit(cfg =>
        {

            cfg.UsingRabbitMq((x, y) =>
               {
                y.Host(rabbitMqOptions.Host, "/", h =>
                {
                    h.Username(rabbitMqOptions.UserName);
                    h.Password(rabbitMqOptions.Password);
                });
            });
        });

        services.AddMassTransitHostedService();

        return services;
    }

}
