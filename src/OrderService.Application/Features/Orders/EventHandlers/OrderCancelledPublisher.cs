namespace OrderService.Application.Features.Orders;

public class OrderCancelledPublisher : INotificationHandler<OrderCancelledDomainEvent>
{
    private readonly IMessageBus _messageBus;

    public OrderCancelledPublisher(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public async Task Handle(OrderCancelledDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var integrationEvent = new OrderCancelledIntegrationEvent(domainEvent.OrderId);
        await _messageBus.PublishEventAsync(integrationEvent, cancellationToken);
    }
}