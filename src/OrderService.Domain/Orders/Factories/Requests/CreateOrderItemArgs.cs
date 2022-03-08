namespace OrderService.Domain.Orders;

public record CreateOrderItemArgs
{
    public string Sku { get; init; } = string.Empty;
    public decimal Quantity { get; init; }
}
