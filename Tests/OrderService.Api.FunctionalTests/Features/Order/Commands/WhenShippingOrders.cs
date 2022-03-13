using OrderService.Contracts.Events.Integration;

namespace OrderService.Api.FunctionalTests.Features.Orders;

[Collection(nameof(TestServerFixtureCollection))]
public class WhenShippingOrders
{
    private readonly Guid Id = Guid.NewGuid();
    private readonly TestServerFixture Given;

    public WhenShippingOrders(TestServerFixture given)
    {
        Given = given ?? throw new Exception("Null Server");
    }

    [Fact]
    [ResetApplicationState]
    public async Task CanShipPaidOrder()
    {
        await Given.AssumeDefaultPaidOrderInRepositoryAsync(Id);

        await Given.Server.CreateClient().PutAndExpectNoContentAsync(PutShipUrl(Id));

        var ordersInDb = await Given.GetAllOrdersInRepository();
        ordersInDb.Should().NotBeNull().And.HaveCount(1);
        var order = ordersInDb.Single();
        order.Status.Should().NotBeNull().And.Be(OrderStatus.Shipped);
    }

    [Fact]
    [ResetApplicationState]
    public async Task WhenOrderIsShippedIntegrationEventIsPublished()
    {
        await Given.AssumeDefaultPaidOrderInRepositoryAsync(Id);

        await Given.Server.CreateClient().PutAndExpectNoContentAsync(PutShipUrl(Id));

        var ordersInDb = await Given.GetAllOrdersInRepository();
        ordersInDb.Should().NotBeNull().And.HaveCount(1);
        var order = ordersInDb.Single();
        order.Status.Should().NotBeNull().And.Be(OrderStatus.Shipped);

        var events = Given.GetPublishedEventsOfType<OrderShippedIntegrationEvent>();
        events.Should().NotBeNull().And.HaveCount(1);
        var integrationEvent = events.First();
        integrationEvent.Should().NotBeNull();
        integrationEvent.Id.Should().Be(Id);
    }

    [Fact]
    [ResetApplicationState]
    public async Task ShipsTheCorrectOrder()
    {
        var anotherOrderId = Guid.NewGuid();
        await Given.AssumeDefaultPaidOrderInRepositoryAsync(Id);
        await Given.AssumeDefaultPaidOrderInRepositoryAsync(anotherOrderId);

        await Given.Server.CreateClient().PutAndExpectNoContentAsync(PutShipUrl(Id));

        var ordersInDb = await Given.GetAllOrdersInRepository();
        ordersInDb.Should().NotBeNull().And.HaveCount(2);
        var changedOrder = ordersInDb.Single(x => x.Id == Id);
        changedOrder.Status.Should().NotBeNull().And.Be(OrderStatus.Shipped);
        var unchangedOrder = ordersInDb.Single(x => x.Id == anotherOrderId);
        unchangedOrder.Status.Should().NotBeNull().And.Be(OrderStatus.Paid);
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsBadRequestWhenOrderHasNotBeingPaidYet()
    {
        await Given.AssumeDefaultOrderInRepositoryAsync(Id);

        await Given.Server.CreateClient().PutAndExpectBadRequestAsync(PutShipUrl(Id));
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsBadRequestWhenOrderIsAlreadyShipped()
    {
        await Given.AssumeDefaultShippedOrderInRepositoryAsync(Id);

        await Given.Server.CreateClient().PutAndExpectBadRequestAsync(PutShipUrl(Id));
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsBadRequestWhenOrderIsCancelled()
    {
        await Given.AssumeDefaultCancelledOrderInRepositoryAsync(Id);

        await Given.Server.CreateClient().PutAndExpectBadRequestAsync(PutShipUrl(Id));
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsNotFoundWhenOrderDontExist()
    {
        var unexistingOrderId = Guid.NewGuid();
        await Given.AssumeDefaultPaidOrderInRepositoryAsync(Id);

        await Given.Server.CreateClient().PutAndExpectNotFoundAsync(PutShipUrl(unexistingOrderId));
    }

    [Fact]
    [ResetApplicationState]
    public async Task NoIntegrationEventIsPublishedWhenOperationFailed()
    {
        await Given.AssumeDefaultShippedOrderInRepositoryAsync(Id);

        await Given.Server.CreateClient().PutAndExpectBadRequestAsync(PutShipUrl(Id));

        var events = Given.GetPublishedEventsOfType<OrderShippedIntegrationEvent>();
        events.Should().NotBeNull().And.BeEmpty();
    }

    #region Fluent Validation 
    [Fact]
    [ResetApplicationState]
    public async Task ReturnsBadRequestWhenOrderIdIsInvalid()
    {
        var invalidOrderId = default(Guid);
        await Given.AssumeDefaultPaidOrderInRepositoryAsync(Id);

        await Given.Server.CreateClient().PutAndExpectBadRequestAsync(PutShipUrl(invalidOrderId));
    }
    #endregion

    private static string PutShipUrl(Guid id) => $"api/orders/{id}/ship";
}
