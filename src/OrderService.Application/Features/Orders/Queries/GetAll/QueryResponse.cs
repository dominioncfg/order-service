namespace OrderService.Application.Features.Orders;

public record GetAllOrdersQueryResponse
{
    public IEnumerable<GetAllOrdersOrderQueryResponse> Orders { get; init; } = Array.Empty<GetAllOrdersOrderQueryResponse>();
}

public record GetAllOrdersOrderQueryResponse
{
    public Guid Id { get; init; }

    public IEnumerable<GetAllOrdersOrderItemQueryResponse> Items { get; init; } = Array.Empty<GetAllOrdersOrderItemQueryResponse>();
}


public record GetAllOrdersOrderItemQueryResponse
{
    public string Sku { get; init; } = string.Empty;
    public decimal Quantity { get; init; }
}
