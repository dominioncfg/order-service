namespace OrderService.Application.Features.Orders;

public record GetOrderByIdQueryResponse
{
    public Guid Id { get; init; }
    public Guid BuyerId { get; init; }
    public DateTime CreationDateTimeUtc { get; init; }
    public GetOrderByIdOrderStatusQueryResponse Status { get; init; } = new();
    public GetOrderByIdOrderAddressQueryResponse Address { get; init; } = new();
    public IEnumerable<GetOrderByIdOrderItemQueryResponse> Items { get; init; } = Array.Empty<GetOrderByIdOrderItemQueryResponse>();
}

public record class GetOrderByIdOrderStatusQueryResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
}

public class GetOrderByIdOrderAddressQueryResponse
{
    public string Country { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string Street { get; init; } = string.Empty;
    public string Number { get; init; } = string.Empty;
}
public record GetOrderByIdOrderItemQueryResponse
{
    public string Sku { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; }
}
