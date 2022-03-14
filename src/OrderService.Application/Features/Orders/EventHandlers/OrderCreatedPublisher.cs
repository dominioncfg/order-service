namespace OrderService.Application.Features.Orders;

public class OrderCreatedPublisher : INotificationHandler<OrderCreatedDomainEvent>
{
    private readonly IMessageBus _messageBus;

    public OrderCreatedPublisher(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public async Task Handle(OrderCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        //TODO: We could use automapper here
        var mappedItems = domainEvent.Items.Select(x => new OrderSubmitedIntegrationEventOrderItemDto(x.Sku, x.UnitPrice, x.Quantity)).ToArray();
        var address = new OrderSubmitedIntegrationEventAddressDto(domainEvent.Address.Country, domainEvent.Address.City,
                                                                 domainEvent.Address.Street, domainEvent.Address.Number);
        var mappedEvent = new OrderSubmitedIntegrationEvent(domainEvent.OrderId, domainEvent.BuyerId, domainEvent.CreationDateTime.UtcValue, address, mappedItems);
        await _messageBus.PublishEventAsync(mappedEvent, cancellationToken);
    }
}
