namespace OrderService.Api.Features.Orders;

public record CreateOrderApiRequest
{
    public Guid Id { get; init; }
    public Guid BuyerId { get; init; }
    public CreateOrderAddressApiRequest Address { get; init; } = new();
    public IEnumerable<CreateOrderItemApiRequest> Items { get; init; } = Array.Empty<CreateOrderItemApiRequest>();
}

public class CreateOrderAddressApiRequest
{
    public string Country { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string Street { get; init; } = string.Empty;
    public string Number { get; init; } = string.Empty;
}

public record CreateOrderItemApiRequest
{
    public string Sku { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; }
}
