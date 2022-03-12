namespace OrderService.Api.FunctionalTests.Features.Orders;

[Collection(nameof(TestServerFixtureCollection))]
public class WhenQueryingOrdersByIds
{
    private readonly TestServerFixture Given;

    public WhenQueryingOrdersByIds(TestServerFixture given)
    {
        Given = given ?? throw new Exception("Null Server");
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsNotFoundWhenOrderDontExist()
    {
        var unexistingOrderId = Guid.NewGuid();
        var existingOrder = new OrderBuilder()
            .WithId(Guid.NewGuid())
            .WithItem(item => item
                .WithSku("product-sku-03")
                .WithQuantity(2)
                .Build()
            )
            .Build();
        await Given.AssumeOrderInRepository(existingOrder);

        var requestUrl = GetOrderUrl(unexistingOrderId);
        await Given.Server.CreateClient().GetAndExpectNotFoundAsync(requestUrl);
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsOrderWithSingleItem()
    {
        var existingOrder = new OrderBuilder()
            .WithId(Guid.NewGuid())
            .WithItem(item => item
                .WithSku("product-sku-03")
                .WithQuantity(2)
                .Build()
            )
            .Build();
        await Given.AssumeOrderInRepository(existingOrder);

        var requestUrl = GetOrderUrl(existingOrder.Id);
        var response = await Given.Server.CreateClient().GetAsync<GetOrderByIdQueryApiResponse>(requestUrl);
        
        response.Items.Should().HaveCount(1);
        AssertOrderByIdApiResponse(response, existingOrder);
    }


    [Fact]
    [ResetApplicationState]
    public async Task ReturnsOrderWithMultipleItems()
    {
        var existingOrder = new OrderBuilder()
            .WithId(Guid.NewGuid())
            .WithItem(item => item
                .WithSku("product-sku-03")
                .WithQuantity(2)
                .Build()
            )
            .WithItem(item => item
                .WithSku("product-sku-04")
                .WithQuantity(3)
                .Build()
            )
            .Build();
        await Given.AssumeOrderInRepository(existingOrder);

        var requestUrl = GetOrderUrl(existingOrder.Id);
        var response = await Given.Server.CreateClient().GetAsync<GetOrderByIdQueryApiResponse>(requestUrl);

        response.Items.Should().HaveCount(2);
        AssertOrderByIdApiResponse(response,existingOrder);
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsCorrectOrderWhenThereMultipleOrders()
    {
        var orderToBeReturned = new OrderBuilder()
            .WithId(Guid.NewGuid())
            .WithItem(item => item
                .WithSku("product-sku-03")
                .WithQuantity(2)
                .Build()
            )
            .Build();
        var orderToBeIgnored = new OrderBuilder()
            .WithId(Guid.NewGuid())
            .WithItem(item => item
                .WithSku("product-sku-03")
                .WithQuantity(2)
            .Build()
       )
       .Build();

        await Given.AssumeOrderInRepository(orderToBeReturned);
        await Given.AssumeOrderInRepository(orderToBeIgnored);

        var requestUrl = GetOrderUrl(orderToBeReturned.Id);
        var response = await Given.Server.CreateClient().GetAsync<GetOrderByIdQueryApiResponse>(requestUrl);

        AssertOrderByIdApiResponse(response, orderToBeReturned);
    }



    private static void AssertOrderByIdApiResponse(GetOrderByIdQueryApiResponse actual, Order expected)
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

    private static string GetOrderUrl(Guid id) => $"api/orders/{id}";
}
