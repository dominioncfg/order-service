namespace OrderService.Domain.Orders;

public class OrderCreationDate: ValueObject
{
    public DateTime Value { get; }

    private OrderCreationDate() { }
    public OrderCreationDate(DateTime creationDayTime)
    {
        if (creationDayTime == DateTime.MinValue || creationDayTime == DateTime.MaxValue)
            throw new InvalidOrderCreationDateTimeDomainException("Order with invalid date");
        Value = creationDayTime;
    }

    protected override IEnumerable<object?> GetEqualityComponents() => new object[] { Value };
}
