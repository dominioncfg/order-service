using OrderService.Consumer.Features.Order;

namespace OrderService.Consumer.Configuration;
internal static class MassTransitConfiguration
{
    public static IServiceCollection AddCustomMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqOptions = new RabbitMqOptions();
        configuration.GetSection(RabbitMqOptions.Section).Bind(rabbitMqOptions);

        services.AddMassTransit(x =>
        {
            x.AddConsumer<OrderSubmitedIntegrationEventConsumer>();
            x.AddConsumer<OrderPaidIntegrationEventConsumer>();
            x.AddConsumer<OrderShippedIntegrationEventConsumer>();
            x.AddConsumer<OrderCancelledIntegrationEventConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqOptions.Host, "/", h =>
                {
                    h.Username(rabbitMqOptions.UserName);
                    h.Password(rabbitMqOptions.Password);
                });
                cfg.ReceiveEndpoint(rabbitMqOptions.QueueName, e =>
                {
                    e.ConfigureConsumer<OrderSubmitedIntegrationEventConsumer>(context);
                    e.ConfigureConsumer<OrderPaidIntegrationEventConsumer>(context);
                    e.ConfigureConsumer<OrderShippedIntegrationEventConsumer>(context);
                    e.ConfigureConsumer<OrderCancelledIntegrationEventConsumer>(context);
                });
            });
        });
        services.AddMassTransitHostedService();

        return services;
    }

}