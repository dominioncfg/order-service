namespace OrderService.Domain.Orders;

public class OrderStatus : Enumeration
{
    public static readonly OrderStatus Submitted = new(1, nameof(Submitted).ToLowerInvariant());
    public static readonly OrderStatus Paid = new(2, nameof(Paid).ToLowerInvariant());
    public static readonly OrderStatus Shipped = new(3, nameof(Shipped).ToLowerInvariant());
    public static readonly OrderStatus Cancelled = new(4, nameof(Cancelled).ToLowerInvariant());

    public OrderStatus(int id, string name) : base(id, name) { }
}
