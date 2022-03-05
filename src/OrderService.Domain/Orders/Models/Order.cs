using OrderService.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderService.Domain.Orders;

public class Order : AggregateRoot
{
    private readonly List<OrderItem> items = new();

    public IReadOnlyList<OrderItem> Items => items.ToList();

    protected Order() { }

    public Order(Guid id, OrderItem[] orderItems) : base(id)
    {
        foreach (var item in orderItems)
            AddOrderItem(item);

        AddDomainEvent(new OrderCreatedDomainEvent()
        {
            OrderId = id,
            Items = Items.Select(x => new OrderItemDto(x.Sku, x.Quantity)).ToArray()
        });
    }

    private void AddOrderItem(OrderItem order)
    {
        items.Add(order);
    }
}
