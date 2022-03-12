namespace OrderService.Api.FunctionalTests.Features.Orders;

public class CreateOrderItemApiRequestBuilder
{
    private string sku = string.Empty;
    private decimal unitPrice;
    private int quantity;

    public CreateOrderItemApiRequestBuilder WithSku(string sku)
    {
        this.sku = sku;
        return this;
    }

    public CreateOrderItemApiRequestBuilder WithQuantity(int quantity)
    {
        this.quantity = quantity;
        return this;
    }
    public CreateOrderItemApiRequestBuilder WithUnitPrice(decimal unitPrice)
    {
        this.unitPrice = unitPrice;
        return this;
    }

    public CreateOrderItemApiRequest Build() => new()
    {
        Sku = sku,
        UnitPrice = unitPrice,
        Quantity = quantity
    };
}