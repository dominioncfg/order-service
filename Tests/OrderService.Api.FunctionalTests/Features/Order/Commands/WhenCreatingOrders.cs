using FluentAssertions;
using OrderService.Api.Features.Orders;
using OrderService.Api.FunctionalTests.SeedWork;
using OrderService.Api.FunctionalTests.Shared;
using OrderService.Contracts;
using OrderService.Domain.Orders;
using OrderService.Tests.Common.Builders;
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
            .WithItem(item=>item
                .WithSku("product-sku-01")
                .WithQuantity(2))
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
    public async Task Returns_Bad_Request_When_Order_Has_Items_With_Negative_Quantity()
    {
        var request = new CreateOrderRequestBuilder()
            .WithId(Guid.NewGuid())
            .WithItem(item=> item
                .WithSku("product-sku-01")
                .WithQuantity(-2)
            )
            .Build();

        await Given.Server.CreateClient().PostAndExpectBadRequestAsync(PostCreateUrl(), request);
    }

    [Fact]
    [ResetApplicationState]
    public async Task Returns_Bad_Request_When_Order_Has_Items_With_Zero_Quantity()
    {
        var request = new CreateOrderRequestBuilder()
            .WithId(Guid.NewGuid())
            .WithItem(item=>item
                .WithSku("product-sku-01")
                .WithQuantity(0)
            )
            .Build();

        await Given.Server.CreateClient().PostAndExpectBadRequestAsync(PostCreateUrl(), request);
    }

    [Fact]
    [ResetApplicationState]
    public async Task Returns_Bad_Request_When_Order_Has_Items_With_Empty_SKU()
    {
        var request = new CreateOrderRequestBuilder()
            .WithId(Guid.NewGuid())
            .WithItem(item=>item
                .WithSku(string.Empty)
                .WithQuantity(2)
            )
            .Build();

        await Given.Server.CreateClient().PostAndExpectBadRequestAsync(PostCreateUrl(), request);
    }

    [Fact]
    [ResetApplicationState]
    public async Task Can_Create_Order_With_Single_Item()
    {
        var request = new CreateOrderRequestBuilder()
            .WithId(Guid.NewGuid())
            .WithItem(item => item
                .WithSku("product-sku-01")
                .WithQuantity(2)
            )
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
            .WithItem(item => item
                .WithSku("product-sku-01")
                .WithQuantity(2)
            )
            .WithItem(item => item
                .WithSku("product-sku-02")
                .WithQuantity(3)
            )
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
            .WithItem(item => item
                .WithSku("product-sku-01")
                .WithQuantity(2)
            )
            .WithItem(item => item
                .WithSku("product-sku-02")
                .WithQuantity(3)
            )
            .Build();
        var existingOrder = new OrderBuilder()
            .WithId(Guid.NewGuid())
            .WithItem(item => item
                .WithSku("product-sku-02")
                .WithQuantity(4)
            )
            .Build();
        await Given.AssumeOrderInRepository(existingOrder);

        await Given.Server.CreateClient().PostAndExpectCreatedAsync(PostCreateUrl(), request);

        var ordersInDb = await Given.GetAllOrdersInRepository();
        ordersInDb.Should().NotBeNull().And.HaveCount(2);
        var theOrder = ordersInDb.FirstOrDefault(x => x.Id == request.Id);
        theOrder.Should().NotBeNull();
        AssertOrderFromRequest(theOrder!, request);
    }

    [Fact]
    [ResetApplicationState]
    public async Task Returns_Bad_Request_When_Id_Already_Exists()
    {
        var existingId = Guid.NewGuid();
        var request = new CreateOrderRequestBuilder()
            .WithId(existingId)
            .WithItem(item => item
                .WithSku("product-sku-01")
                .WithQuantity(2)
            )
            .WithItem(item => item
                .WithSku("product-sku-02")
                .WithQuantity(3)
            )
            .Build();
        var existingOrder = new OrderBuilder()
            .WithId(existingId)
            .WithItem(item => item
                .WithSku("product-sku-03")
                .WithQuantity(4)
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
            .WithItem(item => item
                .WithSku("product-sku-01")
                .WithQuantity(3)
            )
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
    public async Task OrderItems_With_Same_Sku_Are_Combined()
    {
        var differentSku = "product-sku-02";
        var differentSkuQuantity = 2;
        var skuToBeCombined = "product-sku-01";
        var skuToBeCombinedSkuQuantity1 = 3;
        var skuToBeCombinedSkuQuantity2 = 4;
        var request = new CreateOrderRequestBuilder()
            .WithId(Guid.NewGuid())
            .WithItem(item => item
                .WithSku(differentSku)
                .WithQuantity(differentSkuQuantity)
            )
            .WithItem(item => item
                .WithSku(skuToBeCombined)
                .WithQuantity(skuToBeCombinedSkuQuantity1)
            )
            .WithItem(item => item
                .WithSku(skuToBeCombined)
                .WithQuantity(skuToBeCombinedSkuQuantity2)
            )
            .Build();

        await Given.Server.CreateClient().PostAndExpectCreatedAsync(PostCreateUrl(), request);

        var ordersInDb = await Given.GetAllOrdersInRepository();
        ordersInDb.Should().NotBeNull().And.HaveCount(1);
        var orderInDb = ordersInDb.First();

        orderInDb.Id.Should().Be(request.Id);
        orderInDb.Items.Should().HaveCount(2);

        var differentSkuOrderItem = orderInDb.Items.FirstOrDefault(x => x.Sku == differentSku);
        differentSkuOrderItem.Should().NotBeNull();
        differentSkuOrderItem!.Quantity.Should().Be(differentSkuQuantity);

        var combinedOrderItem = orderInDb.Items.FirstOrDefault(x => x.Sku == skuToBeCombined);
        combinedOrderItem.Should().NotBeNull();
        combinedOrderItem!.Quantity.Should().Be(skuToBeCombinedSkuQuantity1 + skuToBeCombinedSkuQuantity2);
    }

    [Fact]
    [ResetApplicationState]
    public async Task Returns_Bad_Request_When_Any_OrderItem_Has_Zero_Quantity_Even_If_Could_Be_Combined()
    {
        var skuToBeCombined = "product-sku-01";
        
        var request = new CreateOrderRequestBuilder()
            .WithId(Guid.NewGuid())
            .WithItem(item => item
                .WithSku(skuToBeCombined)
                .WithQuantity(2)
            )
            .WithItem(item => item
                .WithSku(skuToBeCombined)
                .WithQuantity(0)
            )
            .Build();

        await Given.Server.CreateClient().PostAndExpectBadRequestAsync(PostCreateUrl(), request);
    }

    [Fact]
    [ResetApplicationState]
    public async Task Dont_Send_Event_When_Request_Is_Invalid()
    {
        var request = new CreateOrderRequestBuilder()
            .WithId(Guid.NewGuid())
            .WithItem(item => item
                .WithSku("product-sku-01")
                .WithQuantity(-1)
            )
            .Build();

        await Given.Server.CreateClient().PostAndExpectBadRequestAsync(PostCreateUrl(), request);

        var events = Given.GetPublishedEventsOfType<OrderCreatedIntegrationEvent>();
        events.Should().NotBeNull().And.BeEmpty();
    }

    private static void AssertOrderFromRequest(Order actual, CreateOrderApiRequest expected)
    {
        actual.Should().NotBeNull();
        actual.Id.Should().Be(expected.Id);
        actual.Items.Should().NotBeNull().And.HaveCount(expected.Items.Count());
        foreach (var item in actual.Items)
        {
            var corresponding = actual.Items.FirstOrDefault(x => x.Sku == item.Sku);
            corresponding.Should().NotBeNull();
            item.Quantity.Should().Be(corresponding!.Quantity);
        }
    }

    private static void AssertEventFromRequet(OrderCreatedIntegrationEvent actual, CreateOrderApiRequest expected)
    {
        actual.Should().NotBeNull();
        actual.Id.Should().Be(expected.Id);
        actual.Items.Should().NotBeNull().And.HaveCount(expected.Items.Count());
        foreach (var item in actual.Items)
        {
            var corresponding = actual.Items.FirstOrDefault(x => x.Sku == item.Sku);
            corresponding.Should().NotBeNull();
            item.Quantity.Should().Be(corresponding!.Quantity);
        }
    }

    private static string PostCreateUrl() => "api/orders";
}
