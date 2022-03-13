namespace OrderService.Domain.UnitTests.Orders;

//Prefer functional test over unit test but if the domain logic is to complex or functional test is not posible then use UnitTests
//Prefer test entire aggregates instead of independant classes

public class OrderAggregateCancelOrderTests : OrderAggregateTestsBase
{
    private readonly Guid Id = Guid.NewGuid();

    [Fact]
    public void CanBeCancelledWhenOrderIsSubmitted()
    {
        var order = GivenDefaultOrder(Id);

        order.Cancel();

        order.Status.Should().Be(OrderStatus.Cancelled);
    }

    [Fact]
    public void OrderCancelledDomainEventIsRegistered()
    {
        var order = GivenDefaultPaidOrder(Id);

        order.Cancel();

        order.Status.Should().Be(OrderStatus.Cancelled);
        var cancelledEvents = order.DomainEvents.OfType<OrderCancelledDomainEvent>();
        cancelledEvents.Should().HaveCount(1);
        var cancelEvent = cancelledEvents.First();
        cancelEvent.OrderId.Should().Be(Id);
    }

    [Fact]
    public void CanBeCancelledWhenOrderIsPaid()
    {
        var order = GivenDefaultPaidOrder(Id);

        order.Cancel();

        order.Status.Should().Be(OrderStatus.Cancelled);
    }

    [Fact]
    public void CanBeCancelledWhenOrderIsShipped()
    {
        var order = GivenDefaultShippedOrder(Id);

        order.Cancel();

        order.Status.Should().Be(OrderStatus.Cancelled);
    }

    [Fact]
    public void FailToCancelWhenOrderIsAlreadyCancelled()
    {
        var order = GivenDefaultCancelledOrder(Id);

        Action action = () => order.Cancel();

        action.Should().Throw<OrderCannotBeCancelledDomainException>();
    }
}
