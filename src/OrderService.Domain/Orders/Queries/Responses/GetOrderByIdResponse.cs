namespace OrderService.Domain.Orders;

public record GetOrderByIdResponse
{
    public Guid Id { get; init; }

    public IEnumerable<GetOrderByIdOrderItemResponse> Items { get; init; } = Array.Empty<GetOrderByIdOrderItemResponse>();
}

public record GetOrderByIdOrderItemResponse
{
    public string Sku { get; init; } = string.Empty;
    public decimal Quantity { get; init; }
}
