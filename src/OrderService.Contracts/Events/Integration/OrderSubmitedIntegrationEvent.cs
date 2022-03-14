namespace OrderService.Contracts.Events.Integration;

//OrderService.Contracts could be published as an nuget for other microservices to consume
public class OrderSubmitedIntegrationEvent
{
    public Guid Id { get; }
    public Guid BuyerId { get; init; }
    public DateTime CreationDateTimeUtc { get; init; }
    public OrderSubmitedIntegrationEventAddressDto Address { get; init; }
    public OrderSubmitedIntegrationEventOrderItemDto[] Items { get; }

    public OrderSubmitedIntegrationEvent(Guid id, Guid buyerId, DateTime creationDateTime, OrderSubmitedIntegrationEventAddressDto address, OrderSubmitedIntegrationEventOrderItemDto[] items)
    {
        Id = id;
        BuyerId = buyerId;
        CreationDateTimeUtc = creationDateTime;
        Address = address;
        Items = items;
    }
}

public class OrderSubmitedIntegrationEventAddressDto
{
    public string Country { get; } 
    public string City { get; } 
    public string Street { get; }
    public string Number { get; }


    public OrderSubmitedIntegrationEventAddressDto(string country, string city, string street, string number)
    {
        Country = country;
        City = city;
        Street = street;
        Number = number;
    }
}

public class OrderSubmitedIntegrationEventOrderItemDto
{
    public string Sku { get; }
    public decimal UnitPrice { get; }
    public int Quantity { get; }

    public OrderSubmitedIntegrationEventOrderItemDto(string sku, decimal unitPrice, int quantity)
    {
        Sku = sku;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
}
