namespace OrderService.Application.Features.Orders;

public record CreateOrderCommand : IRequest
{
    public Guid Id { get; init; }
    public Guid BuyerId { get; init; }
    public CreateOrderCommandAddress Address { get; init; } = new();
    public CreateOrderCommandItem[] Items { get; init; } = Array.Empty<CreateOrderCommandItem>();
}

public class CreateOrderCommandAddress
{
    public string Country { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string Street { get; init; } = string.Empty;
    public string Number { get; init; } = string.Empty;
}

public record CreateOrderCommandItem
{
    public string Sku { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; }
}
