namespace OrderService.Domain.Orders;

public class OrderItemUnitPrice : ValueObject
{
    public decimal Value { get; }

    public OrderItemUnitPrice(decimal price)
    {
        if (price <= 0)
            throw new InvalidPriceDomainException($"{price} is an invalid price.");
        Value = price;
    }

    protected override IEnumerable<object?> GetEqualityComponents() => new object[] { Value };
}
