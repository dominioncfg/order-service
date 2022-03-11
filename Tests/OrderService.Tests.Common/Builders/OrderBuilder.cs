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
            CreationDateTime = creationDateTime,
            Address = address!,
            Items = items,
            
        };
        return OrderFactory.Create(args);
    }
}


public class OrderAdressBuilder
{
    private string country = string.Empty;
    private string city = string.Empty;
    private string street = string.Empty;
    private string number = string.Empty;

    public OrderAdressBuilder WithCountry(string country)
    {
        this.country = country;
        return this;
    }

    public OrderAdressBuilder WithCity(string city)
    {
        this.city = city;
        return this;
    }

    public OrderAdressBuilder WithStreet(string street)
    {
        this.street = street;
        return this;
    }

    public OrderAdressBuilder WithNumber(string number)
    {
        this.number = number;
        return this;
    }

    public CreateOrderAddressArgs Build() => new()
    {
        Country = country,
        City = city,
        Street = street,
        Number = number,
    };

}