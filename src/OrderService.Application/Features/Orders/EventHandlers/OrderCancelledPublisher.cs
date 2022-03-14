using MassTransit;
namespace OrderService.Application.Features.Orders;

public class OrderCancelledPublisher : INotificationHandler<OrderCancelledDomainEvent>
{
    //TODO: We could abstract MassTransit if we wanted to:
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderCancelledPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(OrderCancelledDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var integrationEvent = new OrderCancelledIntegrationEvent(domainEvent.OrderId);
        await _publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}
