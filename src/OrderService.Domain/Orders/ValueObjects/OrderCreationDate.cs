namespace OrderService.Domain.Orders;

public class OrderCreationDate : ValueObject
{
    public DateTime UtcValue { get; }

    private OrderCreationDate(DateTime creationDayTime)
    {
        if (creationDayTime == DateTime.MinValue || creationDayTime == DateTime.MaxValue)
            throw new InvalidOrderCreationDateTimeDomainException("Order with invalid date");
        UtcValue = creationDayTime.SetKindUtc();
    }

    public static OrderCreationDate FromUtc(DateTime utcTime) => new(utcTime);

    protected override IEnumerable<object?> GetEqualityComponents() => new object[] { UtcValue };
}
