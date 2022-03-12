namespace OrderService.Api.FunctionalTests.Features.Orders;

public class CreateOrderApiRequestBuilder
{
    private Guid id;
    private Guid buyerId;
    private readonly List<CreateOrderItemApiRequest> items = new();
    private CreateOrderAddressApiRequest? address;

    public CreateOrderApiRequestBuilder WithId(Guid id)
    {
        this.id = id;
        return this;
    }

    public CreateOrderApiRequestBuilder WithBuyerId(Guid buyerId)
    {
        this.buyerId = buyerId;
        return this;
    }

    public CreateOrderApiRequestBuilder WithItem(Action<CreateOrderItemApiRequestBuilder> builderConfig)
    {
        var builder = new CreateOrderItemApiRequestBuilder();
        builderConfig(builder);
        items.Add(builder.Build());
        return this;
    }

    public CreateOrderApiRequestBuilder WithNoItems()
    {
        items.Clear();
        return this;
    }

    public CreateOrderApiRequestBuilder WithAddress(Action<CreateOrderAddressApiRequestBuilder> builderConfig)
    {
        var builder = new CreateOrderAddressApiRequestBuilder();
        builderConfig(builder);
        address = builder.Build();
        return this;
    }

    public CreateOrderApiRequest Build() => new()
    {
        Id = id,
        BuyerId = buyerId,
        Address = address!,
        Items = items,
    };
}

