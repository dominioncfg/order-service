using MassTransit;
namespace OrderService.Application.Features.Orders;

public class OrderShippedPublisher : INotificationHandler<OrderShippedDomainEvent>
{
    //TODO: We could abstract MassTransit if we wanted to:
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderShippedPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(OrderShippedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var integrationEvent = new OrderShippedIntegrationEvent(domainEvent.OrderId);
        await _publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}
