namespace OrderService.Domain.UnitTests.Orders;

//Prefer functional test over unit test but if the domain logic is to complex or functional test is not posible then use UnitTests
//Prefer test entire aggregates instead of independant classes
public class OrderAggregatePayOrderTests : OrderAggregateTestsBase
{
    private readonly Guid Id = Guid.NewGuid();

    [Fact]
    public void CanPaySubmittedOrder()
    {
        var order = GivenDefaultOrder(Id);

        order.MarkAsPaid();

        order.Status.Should().Be(OrderStatus.Paid);
    }

    [Fact]
    public void OrderPaidDomainEventIsRegistered()
    {
        var order = GivenDefaultOrder(Id);

        order.MarkAsPaid();

        order.Status.Should().Be(OrderStatus.Paid);
        var paidEvents = order.DomainEvents.OfType<OrderPaidDomainEvent>();
        paidEvents.Should().HaveCount(1);
        var paidEvent = paidEvents.First();
        paidEvent.OrderId.Should().Be(Id);
    }

    [Fact]
    public void FailToPayWhenOrderIsCanceled()
    {
        var order = GivenDefaultCancelledOrder(Id);

        Action action = () => order.MarkAsPaid();

        action.Should().Throw<OrderCannotBePaidDomainException>();
    }

    [Fact]
    public void FailToPayWhenOrderIsShipped()
    {
        var order = GivenDefaultShippedOrder(Id);

        Action action = () => order.MarkAsPaid();

        action.Should().Throw<OrderCannotBePaidDomainException>();
    }

    [Fact]
    public void FailToPayWhenOrderIsAlreadyPaid()
    {
        var order = GivenDefaultPaidOrder(Id);

        Action action = () => order.MarkAsPaid();

        action.Should().Throw<OrderCannotBePaidDomainException>();
    }  
}
