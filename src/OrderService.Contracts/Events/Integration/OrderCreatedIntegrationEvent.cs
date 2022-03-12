namespace OrderService.Contracts.Events.Integration;

//OrderService.Contracts could be published as an nuget for other microservices to consume
public class OrderCreatedIntegrationEvent
{
    public Guid Id { get; }
    public Guid BuyerId { get; init; }
    public DateTime CreationDateTimeUtc { get; init; }
    public OrderCreatedIntegrationEventAddressDto Address { get; init; } = new();
    public OrderCreatedIntegrationEventOrderItemDto[] Items { get; }

    public OrderCreatedIntegrationEvent(Guid id, Guid buyerId, DateTime creationDateTime, OrderCreatedIntegrationEventAddressDto address, OrderCreatedIntegrationEventOrderItemDto[] items)
    {
        Id = id;
        BuyerId = buyerId;
        CreationDateTimeUtc = creationDateTime;
        Address = address;
        Items = items;
    }
}

public class OrderCreatedIntegrationEventAddressDto
{
    public string Country { get; } = string.Empty;
    public string City { get; } = string.Empty;
    public string Street { get; } = string.Empty;
    public string Number { get; } = string.Empty;

    public OrderCreatedIntegrationEventAddressDto() { }

    public OrderCreatedIntegrationEventAddressDto(string country, string city, string street, string number)
    {
        Country = country;
        City = city;
        Street = street;
        Number = number;
    }
}

public class OrderCreatedIntegrationEventOrderItemDto
{
    public string Sku { get; }
    public decimal UnitPrice { get; }
    public int Quantity { get; }

    public OrderCreatedIntegrationEventOrderItemDto(string sku, decimal unitPrice, int quantity)
    {
        Sku = sku;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
}
