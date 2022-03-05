using System;
namespace OrderService.Contracts;

//OrderService.Contracts could be published as an nuget for other microservices to consume
public class OrderCreatedIntegrationEvent
{
    public Guid Id { get; }

    public OrderCreatedOrderItemDto[] Items { get; }

    public OrderCreatedIntegrationEvent(Guid id, OrderCreatedOrderItemDto[] items)
    {
        this.Id = id;
        this.Items = items;
    }
}

public class OrderCreatedOrderItemDto
{
    public string Sku { get; }
    public decimal Quantity { get; }

    public OrderCreatedOrderItemDto(string sku, decimal quantity)
    {
        Sku = sku;
        Quantity = quantity;
    }
}
