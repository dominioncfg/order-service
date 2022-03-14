namespace OrderService.Application.Features.Orders;

public class OrderPaidPublisher : INotificationHandler<OrderPaidDomainEvent>
{
    private readonly IMessageBus _messageBus;

    public OrderPaidPublisher(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public async Task Handle(OrderPaidDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var integrationEvent = new OrderPaidIntegrationEvent(domainEvent.OrderId);
        await _messageBus.PublishEventAsync(integrationEvent, cancellationToken);
    }
}
