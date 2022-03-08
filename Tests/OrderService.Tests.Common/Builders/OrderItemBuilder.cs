using OrderService.Domain.Orders;

namespace OrderService.Tests.Common.Builders;

public class OrderItemBuilder
{
    private string sku = string.Empty;
    private decimal quantity;

    public OrderItemBuilder WithSku(string sku)
    {
        this.sku = sku;
        return this;
    }

    public OrderItemBuilder WithQuantity(decimal quantity)
    {
        this.quantity = quantity;
        return this;
    }

    public CreateOrderItemArgs Build() => new() { Sku = sku, Quantity = quantity };
}
