using OrderService.Contracts.Events.Integration;

namespace OrderService.Api.FunctionalTests.Features.Orders;

[Collection(nameof(TestServerFixtureCollection))]
public class WhenPayingOrders
{
    private readonly Guid Id = Guid.NewGuid();
    private readonly TestServerFixture Given;

    public WhenPayingOrders(TestServerFixture given)
    {
        Given = given ?? throw new Exception("Null Server");
    }

    [Fact]
    [ResetApplicationState]
    public async Task CanPaySubmittedOrder()
    {
        await Given.AssumeDefaultOrderInRepositoryAsync(Id);

        await Given.Server.CreateClient().PutAndExpectNoContentAsync(PutPayUrl(Id));

        var ordersInDb = await Given.GetAllOrdersInRepository();
        ordersInDb.Should().NotBeNull().And.HaveCount(1);
        var order = ordersInDb.Single();
        order.Status.Should().NotBeNull().And.Be(OrderStatus.Paid);
    }

    [Fact]
    [ResetApplicationState]
    public async Task WhenOrderIsPaidIntegrationEventIsPublished()
    {
        await Given.AssumeDefaultOrderInRepositoryAsync(Id);

        await Given.Server.CreateClient().PutAndExpectNoContentAsync(PutPayUrl(Id));

        var ordersInDb = await Given.GetAllOrdersInRepository();
        ordersInDb.Should().NotBeNull().And.HaveCount(1);
        var order = ordersInDb.Single();
        order.Status.Should().NotBeNull().And.Be(OrderStatus.Paid);

        var events = Given.GetPublishedEventsOfType<OrderPaidIntegrationEvent>();
        events.Should().NotBeNull().And.HaveCount(1);
        var integrationEvent = events.First();
        integrationEvent.Should().NotBeNull();
        integrationEvent.Id.Should().Be(Id);
    }

    [Fact]
    [ResetApplicationState]
    public async Task ChangesTheCorrectOrderPaySubmittedOrder()
    {
        var anotherOrderId = Guid.NewGuid();
        await Given.AssumeDefaultOrderInRepositoryAsync(anotherOrderId);
        await Given.AssumeDefaultOrderInRepositoryAsync(Id);

        await Given.Server.CreateClient().PutAndExpectNoContentAsync(PutPayUrl(Id));

        var ordersInDb = await Given.GetAllOrdersInRepository();
        ordersInDb.Should().NotBeNull().And.HaveCount(2);
        var changedOrder = ordersInDb.Single(x => x.Id == Id);
        changedOrder.Status.Should().NotBeNull().And.Be(OrderStatus.Paid);
        var unchangedOrder = ordersInDb.Single(x => x.Id == anotherOrderId);
        unchangedOrder.Status.Should().NotBeNull().And.Be(OrderStatus.Submitted);
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsNotFoundWhenOrderDontExist()
    {
        var unexistingOrderId = Guid.NewGuid();
        await Given.AssumeDefaultOrderInRepositoryAsync(Id);

        await Given.Server.CreateClient().PutAndExpectNotFoundAsync(PutPayUrl(unexistingOrderId));
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsBadRequestWhenOrderIsAlreadyPaid()
    {
        await Given.AssumeDefaultPaidOrderInRepositoryAsync(Id);

        await Given.Server.CreateClient().PutAndExpectBadRequestAsync(PutPayUrl(Id));
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsBadRequestWhenOrderIsShipped()
    {
        await Given.AssumeDefaultShippedOrderInRepositoryAsync(Id);

        await Given.Server.CreateClient().PutAndExpectBadRequestAsync(PutPayUrl(Id));
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsBadRequestWhenOrderIsCancelled()
    {
        await Given.AssumeDefaultCancelledOrderInRepositoryAsync(Id);

        await Given.Server.CreateClient().PutAndExpectBadRequestAsync(PutPayUrl(Id));
    }

    [Fact]
    [ResetApplicationState]
    public async Task NoIntegrationEventIsPublishedWhenOperationFailed()
    {
        await Given.AssumeDefaultCancelledOrderInRepositoryAsync(Id);

        await Given.Server.CreateClient().PutAndExpectBadRequestAsync(PutPayUrl(Id));
       
        var events = Given.GetPublishedEventsOfType<OrderPaidIntegrationEvent>();
        events.Should().NotBeNull().And.BeEmpty();
    }


    #region Fluent Validation 
    [Fact]
    [ResetApplicationState]
    public async Task ReturnsBadRequestWhenOrderIdIsInvalid()
    {
        var invalidOrderId = default(Guid);
        await Given.AssumeDefaultOrderInRepositoryAsync(Id);

        await Given.Server.CreateClient().PutAndExpectBadRequestAsync(PutPayUrl(invalidOrderId));
    }
    #endregion

    private static string PutPayUrl(Guid id) => $"api/orders/{id}/pay";
}
