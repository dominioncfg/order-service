using MassTransit;
using OrderService.Application.Common.Services;

namespace OrderService.Infrastructure.Services;

public class MassTransitMessageBus : IMessageBus
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitMessageBus(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }
    public async Task PublishEventAsync<T>(T @event, CancellationToken cancellationToken) where T : class
    {
        await _publishEndpoint.Publish(@event, cancellationToken);
    }
}
