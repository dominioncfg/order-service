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
        if (string.IsNullOrEmpty(sku))
            throw new InvalidSkuDomainException("Order with invalid sku");
           
        if(quantity<=0)
            throw new InvalidQuantityDomainException($"{quantity} in order {sku} is invalid.");

        Sku = sku;
        Quantity = quantity;
    }
}
