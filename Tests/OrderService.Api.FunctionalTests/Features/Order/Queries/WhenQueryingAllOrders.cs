using FluentAssertions;
using OrderService.Api.Features.Orders;
using OrderService.Api.FunctionalTests.SeedWork;
using OrderService.Api.FunctionalTests.Shared;
using OrderService.Domain.Orders;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OrderService.Api.FunctionalTests.Features.Orders;

[Collection(nameof(TestServerFixtureCollection))]
public class WhenQueryingAllOrders
{
    private readonly TestServerFixture Given;

    public WhenQueryingAllOrders(TestServerFixture given)
    {
        Given = given ?? throw new Exception("Null Server");
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsEmptyWhenDatabaseIsEmpty()
    {
        var requestUrl = GetOrdersUrl();
        var response = await Given.Server.CreateClient().GetAsync<GetAllOrdersQueryApiResponse>(requestUrl);

        response.Should().NotBeNull();
        response.Orders.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsSingleOrderWithSingleItem()
    {
        var firstOrder = new OrderBuilder()
            .WithId(Guid.NewGuid())
            .WithItem(new OrderItemBuilder()
                .WithSku("product-sku-03")
                .WithQuantity(2)
                .Build()
            )
            .Build();
        await Given.AssumeOrderInRepository(firstOrder);

        var requestUrl = GetOrdersUrl();
        var response = await Given.Server.CreateClient().GetAsync<GetAllOrdersQueryApiResponse>(requestUrl);

        response.Should().NotBeNull();
        response.Orders.Should().HaveCount(1);
        var order = response.Orders.First();
        order.Should().NotBeNull();
        order.Items.Should().HaveCount(1);
        AssertOrderByIdApiResponse(order, firstOrder);
    }


    [Fact]
    [ResetApplicationState]
    public async Task ReturnsSingleOrderWithMultipleItems()
    {
        var firstOrder = new OrderBuilder()
             .WithId(Guid.NewGuid())
             .WithItem(new OrderItemBuilder()
                 .WithSku("product-sku-03")
                 .WithQuantity(2)
                 .Build()
             )
             .WithItem(new OrderItemBuilder()
                 .WithSku("product-sku-04")
                 .WithQuantity(3)
                 .Build()
             )
             .Build();
        await Given.AssumeOrderInRepository(firstOrder);

        var requestUrl = GetOrdersUrl();
        var response = await Given.Server.CreateClient().GetAsync<GetAllOrdersQueryApiResponse>(requestUrl);

        response.Should().NotBeNull();
        response.Orders.Should().HaveCount(1);
        var order = response.Orders.First();
        order.Should().NotBeNull();
        order.Items.Should().HaveCount(2);
        AssertOrderByIdApiResponse(order, firstOrder);
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsMultipleOrdersWithOneItemEach()
    {
        var existingOrders = new[]
        {
            new OrderBuilder()
            .WithId(Guid.NewGuid())
            .WithItem(new OrderItemBuilder()
                .WithSku("product-sku-01")
                .WithQuantity(2)
                .Build()
            )
            .Build(),

            new OrderBuilder()
            .WithId(Guid.NewGuid())
            .WithItem(new OrderItemBuilder()
                .WithSku("product-sku-02")
                .WithQuantity(3)
                .Build()
            )
            .Build()
        };

        foreach (var order in existingOrders)
            await Given.AssumeOrderInRepository(order);

        var requestUrl = GetOrdersUrl();
        var response = await Given.Server.CreateClient().GetAsync<GetAllOrdersQueryApiResponse>(requestUrl);

        response.Should().NotBeNull();
        response.Orders.Should().HaveCount(2);


        foreach (var order in response.Orders)
        {
            order.Should().NotBeNull();
            var expectedOrder = existingOrders.Single(x => x.Id == order.Id);
            order.Items.Should().HaveCount(1);
            AssertOrderByIdApiResponse(order, expectedOrder);
        }
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsMultipleOrdersWithMultipleItemsEach()
    {
        var existingOrders = new[]
        {
            new OrderBuilder()
            .WithId(Guid.NewGuid())
            .WithItem(new OrderItemBuilder()
                .WithSku("product-sku-01")
                .WithQuantity(2)
                .Build()
            )
            .WithItem(new OrderItemBuilder()
               .WithSku("product-sku-02")
               .WithQuantity(3)
               .Build()
            )
            .Build(),

            new OrderBuilder()
            .WithId(Guid.NewGuid())
            .WithItem(new OrderItemBuilder()
                .WithSku("product-sku-03")
                .WithQuantity(4)
                .Build()
            )
            .WithItem(new OrderItemBuilder()
                .WithSku("product-sku-04")
                .WithQuantity(5)
                .Build()
            )
            .Build()
        };

        foreach (var order in existingOrders)
            await Given.AssumeOrderInRepository(order);

        var requestUrl = GetOrdersUrl();
        var response = await Given.Server.CreateClient().GetAsync<GetAllOrdersQueryApiResponse>(requestUrl);

        response.Should().NotBeNull();
        response.Orders.Should().HaveCount(2);


        foreach (var order in response.Orders)
        {
            order.Should().NotBeNull();
            var expectedOrder = existingOrders.Single(x => x.Id == order.Id);
            order.Items.Should().HaveCount(2);
            AssertOrderByIdApiResponse(order, expectedOrder);
        }
    }



    private static void AssertOrderByIdApiResponse(GetAllOrdersOrderQueryApiResponse actual, Order expected)
    {
        actual.Should().NotBeNull();
        actual.Id.Should().Be(expected.Id);
        actual.Items.Should().NotBeNull().And.HaveCount(expected.Items.Count);
        foreach (var item in actual.Items)
        {
            var corresponding = actual.Items.FirstOrDefault(x => x.Sku == item.Sku);
            corresponding.Should().NotBeNull();
            item.Quantity.Should().Be(corresponding!.Quantity);
        }
    }



    private static string GetOrdersUrl() => $"api/orders/";
}
