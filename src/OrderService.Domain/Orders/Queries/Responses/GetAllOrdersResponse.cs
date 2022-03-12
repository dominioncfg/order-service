namespace OrderService.Domain.Orders;

public record GetAllOrdersResponse
{
    public IEnumerable<GetAllOrdersOrderResponse> Orders { get; init; } = Array.Empty<GetAllOrdersOrderResponse>();
}

public record GetAllOrdersOrderResponse
{
    public Guid Id { get; init; }
    public Guid BuyerId { get; init; }
    public DateTime CreationDateTimeUtc { get; init; }
    public GetAllOrdersOrderStatusResponse Status { get; init; } = new();
    public GetAllOrdersOrderAddressResponse Address { get; init; } = new();
    public IEnumerable<GetAllOrdersOrderItemResponse> Items { get; init; } = Array.Empty<GetAllOrdersOrderItemResponse>();
}

public record class GetAllOrdersOrderStatusResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
}

public class GetAllOrdersOrderAddressResponse
{
    public string Country { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string Street { get; init; } = string.Empty;
    public string Number { get; init; } = string.Empty;
}

public record GetAllOrdersOrderItemResponse
{
    public string Sku { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; }
}
