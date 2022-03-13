namespace OrderService.Domain.UnitTests.Orders;

//Prefer functional test over unit test but if the domain logic is to complex or functional test is not posible then use UnitTests
//Prefer test entire aggregates instead of independant classes
public class OrderAggregateShipOrderTests : OrderAggregateTestsBase
{
    private readonly Guid Id = Guid.NewGuid();

    [Fact]
    public void CanBeShippedWhenOrderIsPaid()
    {
        var order = GivenDefaultPaidOrder(Id);

        order.MarkAsShipped();

        order.Status.Should().Be(OrderStatus.Shipped);
    }

    [Fact]
    public void OrderShippedDomainEventIsRegistered()
    {
        var order = GivenDefaultPaidOrder(Id);

        order.MarkAsShipped();

        order.Status.Should().Be(OrderStatus.Shipped);
        var shipEvents = order.DomainEvents.OfType<OrderShippedDomainEvent>();
        shipEvents.Should().HaveCount(1);
        var shipEvent = shipEvents.First();
        shipEvent.OrderId.Should().Be(Id);
    }

    [Fact]
    public void FailToShipWhenOrderIsCanceled()
    {
        var order = GivenDefaultCancelledOrder(Id);

        Action action = () => order.MarkAsShipped();

        action.Should().Throw<OrderCannotBeShippedDomainException>();
    }

    [Fact]
    public void FailToShipWhenOrderIsAlreadyShipped()
    {
        var order = GivenDefaultShippedOrder(Id);

        Action action = () => order.MarkAsShipped();

        action.Should().Throw<OrderCannotBeShippedDomainException>();
    }

    [Fact]
    public void FailToShipWhenOrderHasNonBeenPaid()
    {
        var order = GivenDefaultOrder(Id);

        Action action = () => order.MarkAsShipped();

        action.Should().Throw<OrderCannotBeShippedDomainException>();
    }
}
