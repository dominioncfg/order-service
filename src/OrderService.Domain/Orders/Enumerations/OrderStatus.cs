namespace OrderService.Domain.Orders;

public class OrderStatus : Enumeration
{
    public static readonly OrderStatus Submitted = new(1, nameof(Submitted).ToLowerInvariant());
    public static readonly OrderStatus Paid = new(2, nameof(Paid).ToLowerInvariant());
    public static readonly OrderStatus Shipped = new(3, nameof(Shipped).ToLowerInvariant());
    public static readonly OrderStatus Cancelled = new(4, nameof(Cancelled).ToLowerInvariant());
    private static readonly List<OrderStatus> OrderStatusFlowInOrder = new[] { Submitted, Paid, Shipped }.ToList();

    public OrderStatus(int id, string name) : base(id, name) { }

    public bool CanChangeTo(OrderStatus candiateStatus)
    {
        if (this == Cancelled)
            return false;

        if (candiateStatus == Cancelled)
            return true;

        var actualStatusIndex = OrderStatusFlowInOrder.IndexOf(this);
        var changeToStatusIndex = OrderStatusFlowInOrder.IndexOf(candiateStatus);

        return changeToStatusIndex - actualStatusIndex == 1;
    }
}
