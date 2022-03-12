namespace OrderService.Application.Features.Orders;

public record GetOrderByIdQueryResponse
{
    public Guid Id { get; init; }

    public IEnumerable<GetOrderByIdOrderItemQueryResponse> Items { get; init; } = Array.Empty<GetOrderByIdOrderItemQueryResponse>();
}


public record GetOrderByIdOrderItemQueryResponse
{
    public string Sku { get; init; } = string.Empty;
    public decimal Quantity { get; init; }
}
