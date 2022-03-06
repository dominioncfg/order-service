namespace OrderService.Domain.Orders;

public class OrderItem
{
    public string Sku { get; }
    public decimal Quantity { get; }

#nullable disable
    protected OrderItem() { }
#nullable enable

    public OrderItem(string sku, decimal quantity)
    {
        Sku = sku;
        Quantity = quantity;
    }
}
