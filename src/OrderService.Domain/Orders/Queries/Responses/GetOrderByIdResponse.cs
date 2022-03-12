namespace OrderService.Domain.Orders;

public record GetOrderByIdResponse
{
    public Guid Id { get; init; }
    public Guid BuyerId { get; init; }
    public DateTime CreationDateTimeUtc { get; init; }
    public GetOrderByIdOrderStatusResponse Status { get; init; } = new();
    public GetOrderByIdOrderAddressResponse Address { get; init; } = new();
    public IEnumerable<GetOrderByIdOrderItemResponse> Items { get; init; } = Array.Empty<GetOrderByIdOrderItemResponse>();
}

public record class GetOrderByIdOrderStatusResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
}

public class GetOrderByIdOrderAddressResponse
{
    public string Country { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string Street { get; init; } = string.Empty;
    public string Number { get; init; } = string.Empty;
}

public record GetOrderByIdOrderItemResponse
{
    public string Sku { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; }
}
