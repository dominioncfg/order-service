namespace OrderService.Domain.Orders;

public record OrderCreatedDomainEvent : IDomainEvent
{
    public Guid OrderId { get; init; }
    public Guid BuyerId { get; init; }
    public OrderCreationDate CreationDateTime { get; init; }
    public OrderAddress Address { get; init; }
    public IEnumerable<OrderCreatedOrderItemDomainEventDto> Items { get; init; }

    private OrderCreatedDomainEvent(Guid orderId, Guid buyerId, OrderCreationDate creationDateTime, OrderAddress address, OrderCreatedOrderItemDomainEventDto[] items)
    {
        OrderId = orderId;
        BuyerId = buyerId;
        Address = address;
        CreationDateTime = creationDateTime;
        Items = items;
    }
    public static OrderCreatedDomainEvent FromOrder(Order order)
    {
        var items = order.Items
            .Select(x => new OrderCreatedOrderItemDomainEventDto(x.Id, x.Sku.Value, x.UnitPrice.PriceInEuros, x.Quantity.Value))
            .ToArray();
        return new OrderCreatedDomainEvent(order.Id, order.BuyerId, order.CreationDateTime,order.Address, items);
    }
}

public record OrderCreatedOrderItemDomainEventDto(Guid Id, string Sku, decimal UnitPrice, int Quantity);
