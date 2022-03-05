using OrderService.Domain.Orders;
using System;
using System.Collections.Generic;

namespace OrderService.Api.FunctionalTests.Shared;

public class OrderBuilder
{
    private Guid id;
    private readonly List<OrderItem> items = new();


    public OrderBuilder WithId(Guid id)
    {
        this.id = id;
        return this;
    }

    public OrderBuilder WithItem(OrderItem item)
    {
        this.items.Add(item);
        return this;
    }

    public OrderBuilder WithItems(params OrderItem[] items)
    {
        this.items.AddRange(items);
        return this;
    }

    public Order Build()
    {
        var order = new Order(id, items.ToArray());
        return order;
    }

}
