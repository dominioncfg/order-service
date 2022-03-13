using OrderService.Contracts.Events.Integration;

namespace OrderService.Api.FunctionalTests.Features.Orders;

[Collection(nameof(TestServerFixtureCollection))]
public class WhenCancellingOrders
{
    private readonly Guid Id = Guid.NewGuid();
    private readonly TestServerFixture Given;

    public WhenCancellingOrders(TestServerFixture given)
    {
        Given = given ?? throw new Exception("Null Server");
    }

    [Fact]
    [ResetApplicationState]
    public async Task CanCancelSubmittedOrder()
    {
        await Given.AssumeDefaultOrderInRepositoryAsync(Id);

        await Given.Server.CreateClient().PutAndExpectNoContentAsync(PutCancelUrl(Id));

        var ordersInDb = await Given.GetAllOrdersInRepository();
        ordersInDb.Should().NotBeNull().And.HaveCount(1);
        var order = ordersInDb.Single();
        order.Status.Should().NotBeNull().And.Be(OrderStatus.Cancelled);
    }

    [Fact]
    [ResetApplicationState]
    public async Task WhenOrderIsCancelledIntegrationEventIsPublished()
    {
        await Given.AssumeDefaultOrderInRepositoryAsync(Id);

        await Given.Server.CreateClient().PutAndExpectNoContentAsync(PutCancelUrl(Id));

        var ordersInDb = await Given.GetAllOrdersInRepository();
        ordersInDb.Should().NotBeNull().And.HaveCount(1);
        var order = ordersInDb.Single();
        order.Status.Should().NotBeNull().And.Be(OrderStatus.Cancelled);

        var events = Given.GetPublishedEventsOfType<OrderCanceledIntegrationEvent>();
        events.Should().NotBeNull().And.HaveCount(1);
        var integrationEvent = events.First();
        integrationEvent.Should().NotBeNull();
        integrationEvent.Id.Should().Be(Id);
    }

    [Fact]
    [ResetApplicationState]
    public async Task CancelsTheCorrectOrder()
    {
        var anotherOrderId = Guid.NewGuid();
        await Given.AssumeDefaultOrderInRepositoryAsync(Id);
        await Given.AssumeDefaultOrderInRepositoryAsync(anotherOrderId);

        await Given.Server.CreateClient().PutAndExpectNoContentAsync(PutCancelUrl(Id));

        var ordersInDb = await Given.GetAllOrdersInRepository();
        ordersInDb.Should().NotBeNull().And.HaveCount(2);
        var changedOrder = ordersInDb.Single(x => x.Id == Id);
        changedOrder.Status.Should().NotBeNull().And.Be(OrderStatus.Cancelled);
        var unchangedOrder = ordersInDb.Single(x => x.Id == anotherOrderId);
        unchangedOrder.Status.Should().NotBeNull().And.Be(OrderStatus.Submitted);
    }

    [Fact]
    [ResetApplicationState]
    public async Task CanCancelPaidOrder()
    {
        await Given.AssumeDefaultPaidOrderInRepositoryAsync(Id);

        await Given.Server.CreateClient().PutAndExpectNoContentAsync(PutCancelUrl(Id));

        var ordersInDb = await Given.GetAllOrdersInRepository();
        ordersInDb.Should().NotBeNull().And.HaveCount(1);
        var order = ordersInDb.Single();
        order.Status.Should().NotBeNull().And.Be(OrderStatus.Cancelled);
    }

    [Fact]
    [ResetApplicationState]
    public async Task CanCancelShippedOrder()
    {
        await Given.AssumeDefaultShippedOrderInRepositoryAsync(Id);

        await Given.Server.CreateClient().PutAndExpectNoContentAsync(PutCancelUrl(Id));

        var ordersInDb = await Given.GetAllOrdersInRepository();
        ordersInDb.Should().NotBeNull().And.HaveCount(1);
        var order = ordersInDb.Single();
        order.Status.Should().NotBeNull().And.Be(OrderStatus.Cancelled);
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsBadRequestWhenOrderIsAlreadyCancelled()
    {
        await Given.AssumeDefaultCancelledOrderInRepositoryAsync(Id);

        await Given.Server.CreateClient().PutAndExpectBadRequestAsync(PutCancelUrl(Id));
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsNotFoundWhenOrderDontExist()
    {
        var unexistingOrderId = Guid.NewGuid();
        await Given.AssumeDefaultOrderInRepositoryAsync(Id);

        await Given.Server.CreateClient().PutAndExpectNotFoundAsync(PutCancelUrl(unexistingOrderId));
    }


    [Fact]
    [ResetApplicationState]
    public async Task NoIntegrationEventIsPublishedWhenOperationFailed()
    {
        await Given.AssumeDefaultCancelledOrderInRepositoryAsync(Id);

        await Given.Server.CreateClient().PutAndExpectBadRequestAsync(PutCancelUrl(Id));

        var events = Given.GetPublishedEventsOfType<OrderCanceledIntegrationEvent>();
        events.Should().NotBeNull().And.BeEmpty();
    }
    #region Fluent Validation 
    [Fact]
    [ResetApplicationState]
    public async Task ReturnsBadRequestWhenOrderIdIsInvalid()
    {
        var invalidOrderId = default(Guid);
        await Given.AssumeDefaultOrderInRepositoryAsync(Id);

        await Given.Server.CreateClient().PutAndExpectBadRequestAsync(PutCancelUrl(invalidOrderId));
    }
    #endregion

    private static string PutCancelUrl(Guid id) => $"api/orders/{id}/cancel";
}
