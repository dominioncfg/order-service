namespace OrderService.Domain.Orders;

public class OrderItem : Entity<Guid>
{
    public OrderItemSku Sku { get; }
    public OrderItemUnitPrice UnitPrice { get; }
    public OrderItemQuantity Quantity { get; }
    
#nullable disable
    protected OrderItem()
    { }
#nullable enable

    public OrderItem(Guid id, string sku, decimal unitPrice, int quantity)
    {
        Id = id;
        Sku = new OrderItemSku(sku);
        UnitPrice = new OrderItemUnitPrice(unitPrice);
        Quantity = new OrderItemQuantity(quantity);
    }
}


