using OrderService.Api.Features.Orders;

namespace OrderService.Api.FunctionalTests.Features.Orders;

public class CreateOrderItemApiRequestBuilder
{
    private string sku = string.Empty;
    private decimal quantity;

    public CreateOrderItemApiRequestBuilder WithSku(string sku)
    {
        this.sku = sku;
        return this;
    }

    public CreateOrderItemApiRequestBuilder WithQuantity(decimal quantity)
    {
        this.quantity = quantity;
        return this;
    }

    public CreateOrderItemApiRequest Build() => new() { Sku = sku, Quantity = quantity };
}