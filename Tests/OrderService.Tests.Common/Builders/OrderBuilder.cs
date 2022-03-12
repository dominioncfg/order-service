using OrderService.Domain.Orders;

namespace OrderService.Tests.Common.Builders;

public class OrderBuilder
{
    private Guid id;
    private Guid buyerId;
    private DateTime creationDateTime;
    private CreateOrderAddressArgs? address;
    private readonly List<CreateOrderItemArgs> items = new();

    public OrderBuilder WithId(Guid id)
    {
        this.id = id;
        return this;
    }

    public OrderBuilder WithBuyerId(Guid buyerId)
    {
        this.buyerId = buyerId;
        return this;
    }

    public OrderBuilder WithCreationDateTime(DateTime creationDateTime)
    {
        this.creationDateTime = creationDateTime;
        return this;
    }

    public OrderBuilder WithItem(Action<OrderItemBuilder> builderConfig)
    {
        var builder = new OrderItemBuilder();
        builderConfig(builder);
        items.Add(builder.Build());
        return this;
    }

    public OrderBuilder WithAddress(Action<OrderAdressBuilder> builderConfig)
    {
        var builder = new OrderAdressBuilder();
        builderConfig(builder);
        address = builder.Build();
        return this;
    }

    public Order Build()
    {
        var args = new CreateOrderArgs()
        {
            Id = id,
            BuyerId = buyerId,
            CreationDateTimeUtc = creationDateTime,
            Address = address!,
            Items = items,
        };
        return OrderFactory.Create(args);
    }
}
