namespace OrderService.Domain.Orders;

public record CreateOrderArgs
{
    public Guid Id { get; init; }
    public Guid BuyerId { get; init; }
    public IEnumerable<CreateOrderItemArgs> Items { get; init; } = Array.Empty<CreateOrderItemArgs>();
    public DateTime CreationDateTimeUtc { get; init; }
    public CreateOrderAddressArgs Address { get; init; } = new();
}
