using OrderService.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderService.Domain.Orders;

public record OrderCreatedDomainEvent : IDomainEvent
{
    public Guid OrderId { get; init; }
    public IEnumerable<OrderItemDto> Items { get; init; } 

    private OrderCreatedDomainEvent(Guid orderId, OrderItemDto[] items) 
    { 
        OrderId = orderId;
        Items = items;
    }
    public static OrderCreatedDomainEvent FromOrder(Order order)
    {
        var items = order.Items
            .Select(x => new OrderItemDto(x.Sku, x.Quantity))
            .ToArray();
        return new OrderCreatedDomainEvent(order.Id,items);
    }
}

public record OrderItemDto(string Sku, decimal Quantity);
