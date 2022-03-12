namespace OrderService.Api.Features.Orders;

public record GetAllOrdersQueryApiResponse
{
    public IEnumerable<GetAllOrdersOrderQueryApiResponse> Orders { get; init; } = Array.Empty<GetAllOrdersOrderQueryApiResponse>();
}

public record GetAllOrdersOrderQueryApiResponse
{
    public Guid Id { get; init; }
    public Guid BuyerId { get; init; }
    public DateTime CreationDateTimeUtc { get; init; }
    public GetAllOrdersOrderStatusQueryApiResponse Status { get; init; } = new();
    public GetAllOrdersOrderAddressQueryApiResponse Address { get; init; } = new();

    public IEnumerable<GetAllOrdersOrderItemQueryApiResponse> Items { get; init; } = Array.Empty<GetAllOrdersOrderItemQueryApiResponse>();
}

public record class GetAllOrdersOrderStatusQueryApiResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
}

public class GetAllOrdersOrderAddressQueryApiResponse
{
    public string Country { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string Street { get; init; } = string.Empty;
    public string Number { get; init; } = string.Empty;
}


public record GetAllOrdersOrderItemQueryApiResponse
{
    public string Sku { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; }
}

