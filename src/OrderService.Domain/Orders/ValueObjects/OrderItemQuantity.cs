namespace OrderService.Domain.Orders;

public class OrderItemQuantity : ValueObject
{
    public int Value { get; }

    public OrderItemQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new InvalidQuantityDomainException($"'{quantity}' is an invalid quantity.");
        Value = quantity;
    }

    protected override IEnumerable<object?> GetEqualityComponents() => new object[] { Value };
}


