using OrderService.Domain.Orders;
using System;
using System.Collections.Generic;

namespace OrderService.Api.FunctionalTests.Shared;

public class OrderBuilder
{
    private Guid id;
    private readonly List<CreateOrderItemArgs> items = new();

    public OrderBuilder WithId(Guid id)
    {
        this.id = id;
        return this;
    }
    
    public OrderBuilder WithItem(Action<OrderItemBuilder> builderConfig)
    {
        var builder =  new OrderItemBuilder();
        builderConfig(builder);
        items.Add(builder.Build());
        return this;
    }

    public Order Build()
    {
        var args = new CreateOrderArgs()
        {
            Id = id,
            Items = items,
        };
        return OrderFactory.Create(args);
    }
}
