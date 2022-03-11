namespace OrderService.Domain.Orders;

public class OrderItemSku : ValueObject
{
    public string Value { get; }

    public OrderItemSku(string sku)
    {
        if (string.IsNullOrEmpty(sku))
            throw new InvalidSkuDomainException("Order with invalid sku");
        Value = sku;
    }

    protected override IEnumerable<object?> GetEqualityComponents()=> new[] { Value };
}
