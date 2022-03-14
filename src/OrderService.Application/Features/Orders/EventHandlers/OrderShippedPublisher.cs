namespace OrderService.Application.Features.Orders;

public class OrderShippedPublisher : INotificationHandler<OrderShippedDomainEvent>
{
    private readonly IMessageBus _messageBus;

    public OrderShippedPublisher(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public async Task Handle(OrderShippedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var integrationEvent = new OrderShippedIntegrationEvent(domainEvent.OrderId);
        await _messageBus.PublishEventAsync(integrationEvent, cancellationToken);
    }
}
