namespace OrderService.Application.Features.Orders;

public record GetAllOrdersQueryResponse
{
    public IEnumerable<GetAllOrdersOrderQueryResponse> Orders { get; init; } = Array.Empty<GetAllOrdersOrderQueryResponse>();
}

public record GetAllOrdersOrderQueryResponse
{
    public Guid Id { get; init; }
    public Guid BuyerId { get; init; }
    public DateTime CreationDateTimeUtc { get; init; }
    public GetAllOrdersOrderStatusQueryResponse Status { get; init; } = new();
    public GetAllOrdersOrderAddressQueryResponse Address { get; init; } = new();
    public IEnumerable<GetAllOrdersOrderItemQueryResponse> Items { get; init; } = Array.Empty<GetAllOrdersOrderItemQueryResponse>();
}

public record class GetAllOrdersOrderStatusQueryResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
}

public class GetAllOrdersOrderAddressQueryResponse
{
    public string Country { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string Street { get; init; } = string.Empty;
    public string Number { get; init; } = string.Empty;
}

public record GetAllOrdersOrderItemQueryResponse
{
    public string Sku { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; }
}
