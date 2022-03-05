using FluentAssertions;
using OrderService.Api.Features.Orders;
using OrderService.Api.FunctionalTests.SeedWork;
using OrderService.Api.FunctionalTests.Shared;
using OrderService.Contracts;
using OrderService.Domain.Orders;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OrderService.Api.FunctionalTests.Features.Orders;

[Collection(nameof(TestServerFixtureCollection))]
public class WhenCreatingOrders
{
    private readonly TestServerFixture Given;

    public WhenCreatingOrders(TestServerFixture given)
    {
        Given = given ?? throw new Exception("Null Server");
    }

    [Fact]
    [ResetApplicationState]
    public async Task Returns_Bad_Request_When_Id_Is_Not_Valid()
    {
        var request = new CreateOrderRequestBuilder()
            .WithId(default)
            .WithItem("product-sku-01", 2)
            .Build();

        await Given.Server.CreateClient().PostAndExpectBadRequestAsync(PostCreateUrl(), request);
    }

    [Fact]
    [ResetApplicationState]
    public async Task Returns_Bad_Request_When_Order_Has_No_Items()
    {
        var request = new CreateOrderRequestBuilder()
            .WithId(Guid.NewGuid())
            .Build();

        await Given.Server.CreateClient().PostAndExpectBadRequestAsync(PostCreateUrl(), request);
    }

    [Fact]
    [ResetApplicationState]
    public async Task Returns_Bad_Request_When_Order_Has_Items_With_Negative_Value()
    {
        var request = new CreateOrderRequestBuilder()
            .WithId(Guid.NewGuid())
            .WithItem("product-sku-01", -2)
            .Build();

        await Given.Server.CreateClient().PostAndExpectBadRequestAsync(PostCreateUrl(), request);
    }

    [Fact]
    [ResetApplicationState]
    public async Task Returns_Bad_Request_When_Order_Has_Items_With_Empty_SKU()
    {
        var request = new CreateOrderRequestBuilder()
            .WithId(Guid.NewGuid())
            .WithItem("", 2)
            .Build();

        await Given.Server.CreateClient().PostAndExpectBadRequestAsync(PostCreateUrl(), request);
    }

    [Fact]
    [ResetApplicationState]
    public async Task Can_Create_Order_With_Single_Item()
    {
        var request = new CreateOrderRequestBuilder()
            .WithId(Guid.NewGuid())
            .WithItem("product-sku-01", 2)
            .Build();

        await Given.Server.CreateClient().PostAndExpectCreatedAsync(PostCreateUrl(), request);

        var ordersInDb = await Given.GetAllOrdersInRepository();
        ordersInDb.Should().NotBeNull().And.HaveCount(1);
        AssertOrderFromRequest(ordersInDb.First(), request);
    }

    [Fact]
    [ResetApplicationState]
    public async Task Can_Create_Order_With_Two_Items()
    {
        var request = new CreateOrderRequestBuilder()
            .WithId(Guid.NewGuid())
            .WithItem("product-sku-01", 2)
            .WithItem("product-sku-02", 3)
            .Build();

        await Given.Server.CreateClient().PostAndExpectCreatedAsync(PostCreateUrl(), request);

        var ordersInDb = await Given.GetAllOrdersInRepository();
        ordersInDb.Should().NotBeNull().And.HaveCount(1);
        AssertOrderFromRequest(ordersInDb.First(), request);
    }

    [Fact]
    [ResetApplicationState]
    public async Task Can_Create_Order_When_Another_Order_Database_Exist()
    {
        var request = new CreateOrderRequestBuilder()
            .WithId(Guid.NewGuid())
            .WithItem("product-sku-01", 2)
            .WithItem("product-sku-02", 3)
            .Build();
        var existingOrder = new OrderBuilder()
            .WithId(Guid.NewGuid())
            .WithItem(new OrderItemBuilder()
                .WithSku("product-sku-03")
                .WithQuantity(2)
                .Build()
            )
            .Build();
        await Given.AssumeOrderInRepository(existingOrder);

        await Given.Server.CreateClient().PostAndExpectCreatedAsync(PostCreateUrl(), request);

        var ordersInDb = await Given.GetAllOrdersInRepository();
        ordersInDb.Should().NotBeNull().And.HaveCount(2);
        AssertOrderFromRequest(ordersInDb.FirstOrDefault(x => x.Id == request.Id), request);
    }

    [Fact]
    [ResetApplicationState]
    public async Task Returns_Bad_Request_When_Id_Already_Exists()
    {
        var existingId = Guid.NewGuid();
        var request = new CreateOrderRequestBuilder()
            .WithId(existingId)
            .WithItem("product-sku-01", 2)
            .WithItem("product-sku-02", 3)
            .Build();
        var existingOrder = new OrderBuilder()
            .WithId(existingId)
            .WithItem(new OrderItemBuilder()
                .WithSku("product-sku-03")
                .WithQuantity(2)
                .Build()
            )
            .Build();
        await Given.AssumeOrderInRepository(existingOrder);

        await Given.Server.CreateClient().PostAndExpectBadRequestAsync(PostCreateUrl(), request);
    }

    [Fact]
    [ResetApplicationState]
    public async Task Sends_Event_When_Order_Is_Created()
    {
        var request = new CreateOrderRequestBuilder()
            .WithId(Guid.NewGuid())
            .WithItem("product-sku-01", 2)
            .Build();

        await Given.Server.CreateClient().PostAndExpectCreatedAsync(PostCreateUrl(), request);

        var ordersInDb = await Given.GetAllOrdersInRepository();

        var events = Given.GetPublishedEventsOfType<OrderCreatedIntegrationEvent>();
        ordersInDb.Should().NotBeNull().And.HaveCount(1);
        events.Should().NotBeNull().And.HaveCount(1);
        AssertOrderFromRequest(ordersInDb.First(), request);
        AssertEventFromRequet(events.First(), request);
    }

    [Fact]
    [ResetApplicationState]
    public async Task Dont_Send_Event_When_Request_Is_Invalid()
    {
        var request = new CreateOrderRequestBuilder()
            .WithId(Guid.NewGuid())
            .WithItem("product-sku-01", -1)
            .Build();

        await Given.Server.CreateClient().PostAndExpectBadRequestAsync(PostCreateUrl(), request);

        var events = Given.GetPublishedEventsOfType<OrderCreatedIntegrationEvent>();
        events.Should().NotBeNull().And.BeEmpty();
    }

    private static void AssertOrderFromRequest(Order actual, CreateOrderRequest expected)
    {
        actual.Should().NotBeNull();
        actual.Id.Should().Be(expected.Id);
        actual.Items.Should().NotBeNull().And.HaveCount(expected.Items.Count);
        foreach (var item in actual.Items)
        {
            var corresponding = actual.Items.FirstOrDefault(x => x.Sku == item.Sku);
            corresponding.Should().NotBeNull();
            item.Quantity.Should().Be(corresponding.Quantity);
        }
    }

    private static void AssertEventFromRequet(OrderCreatedIntegrationEvent actual, CreateOrderRequest expected)
    {
        actual.Should().NotBeNull();
        actual.Id.Should().Be(expected.Id);
        actual.Items.Should().NotBeNull().And.HaveCount(expected.Items.Count);
        foreach (var item in actual.Items)
        {
            var corresponding = actual.Items.FirstOrDefault(x => x.Sku == item.Sku);
            corresponding.Should().NotBeNull();
            item.Quantity.Should().Be(corresponding.Quantity);
        }
    }

    private static string PostCreateUrl() => "api/orders";
}
