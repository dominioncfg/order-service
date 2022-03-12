namespace OrderService.Api.Features.Orders;

public record GetOrderByIdQueryApiResponse
{
    public Guid Id { get; init; }
    public Guid BuyerId { get; init; }
    public DateTime CreationDateTimeUtc { get; init; }
    public GetOrderByIdOrderStatusQueryApiResponse Status { get; init; } = new();
    public GetOrderByIdOrderAddressQueryApiResponse Address { get; init; } = new();
    public IEnumerable<GetOrderByIdOrderItemQueryApiResponse> Items { get; init; } = Array.Empty<GetOrderByIdOrderItemQueryApiResponse>();
}

public record class GetOrderByIdOrderStatusQueryApiResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
}

public class GetOrderByIdOrderAddressQueryApiResponse
{
    public string Country { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string Street { get; init; } = string.Empty;
    public string Number { get; init; } = string.Empty;
}

public record GetOrderByIdOrderItemQueryApiResponse
{
    public string Sku { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; }
}

