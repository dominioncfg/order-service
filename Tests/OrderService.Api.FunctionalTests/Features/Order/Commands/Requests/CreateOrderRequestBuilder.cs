using OrderService.Api.Features.Orders;
using System;
using System.Collections.Generic;

namespace OrderService.Api.FunctionalTests.Features.Orders;

public class CreateOrderRequestBuilder
{
    private Guid id;
    private readonly List<CreateOrderItemApiRequest> items = new();

    public CreateOrderRequestBuilder WithId(Guid id)
    {
        this.id = id;
        return this;
    }

    public CreateOrderRequestBuilder WithItem(Action<CreateOrderItemApiRequestBuilder> builderConfig)
    {
        CreateOrderItemApiRequestBuilder builder = new CreateOrderItemApiRequestBuilder();
        builderConfig(builder);
        items.Add(builder.Build());
        return this;
    }

    public CreateOrderApiRequest Build() => new() { Id = id, Items = items };
}
