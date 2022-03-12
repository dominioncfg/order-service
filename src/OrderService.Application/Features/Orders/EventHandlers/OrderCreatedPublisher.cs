using MassTransit;
namespace OrderService.Application.Features.Orders;

public class OrderCreatedPublisher : INotificationHandler<OrderCreatedDomainEvent>
{
    //TODO: We could abstract MassTransit if we wanted to:
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderCreatedPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(OrderCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var mappedItems = domainEvent.Items.Select(x => new OrderCreatedIntegrationEventOrderItemDto(x.Sku, x.UnitPrice, x.Quantity)).ToArray();
        var address = new OrderCreatedIntegrationEventAddressDto(domainEvent.Address.Country, domainEvent.Address.City,
                                                                 domainEvent.Address.Street, domainEvent.Address.Number);
        var mappedEvent = new OrderCreatedIntegrationEvent(domainEvent.OrderId, domainEvent.BuyerId, domainEvent.CreationDateTime.UtcValue, address, mappedItems);
        await _publishEndpoint.Publish(mappedEvent, cancellationToken);
    }
}
