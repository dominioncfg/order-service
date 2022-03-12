namespace OrderService.Api.Features.Orders;

public record GetOrderByIdQueryApiResponse
{
    public Guid Id { get; init; }

    public IEnumerable<GetOrderByIdOrderItemQueryApiResponse> Items { get; init; } = Array.Empty<GetOrderByIdOrderItemQueryApiResponse>();
}

public record GetOrderByIdOrderItemQueryApiResponse
{
    public string Sku { get; init; } = string.Empty;
    public decimal Quantity { get; init; }
}

