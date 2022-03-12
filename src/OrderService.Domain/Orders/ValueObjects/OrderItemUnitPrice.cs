namespace OrderService.Domain.Orders;

public class OrderItemUnitPrice : ValueObject
{
    public decimal PriceInEuros { get; }

    public OrderItemUnitPrice(decimal price)
    {
        if (price <= 0)
            throw new InvalidPriceDomainException($"{price} is an invalid price.");
        PriceInEuros = price;
    }

    public static OrderItemUnitPrice FromEuros(decimal euros) => new(euros);

    protected override IEnumerable<object?> GetEqualityComponents() => new object[] { PriceInEuros };
}
