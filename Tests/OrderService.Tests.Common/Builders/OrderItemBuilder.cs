using OrderService.Domain.Orders;

namespace OrderService.Tests.Common.Builders;

public class OrderItemBuilder
{
    private string sku = string.Empty;
    private decimal unitPrice;
    private int quantity;

    public OrderItemBuilder WithSku(string sku)
    {
        this.sku = sku;
        return this;
    }

    public OrderItemBuilder WithQuantity(int quantity)
    {
        this.quantity = quantity;
        return this;
    }

    public OrderItemBuilder WithUnitPrice(decimal unitPrice)
    {
        this.unitPrice = unitPrice;
        return this;
    }

    public CreateOrderItemArgs Build() => new() { Sku = sku, UnitPrice = unitPrice, Quantity = quantity };
}
