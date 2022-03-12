namespace OrderService.Api.FunctionalTests.Features.Orders;

[Collection(nameof(TestServerFixtureCollection))]
public class WhenQueryingOrdersByIds
{
    private readonly Guid Id = Guid.NewGuid();
    private readonly Guid BuyerId = Guid.NewGuid();
    private readonly DateTime CreationDateTime = new DateTime(2020, 10, 02).At(03, 00);
    private const string Sku = "prod01";
    private const decimal UnitPrice = 10;
    private const int Quantity = 1;
    private const string AddressCountry = "Spain";
    private const string AddressCity = "Madrid";
    private const string AddressStreet = "Gran Via";
    private const string AddressNumber = "55";
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
            .WithId(Id)
            .WithBuyerId(BuyerId)
            .WithCreationDateTime(CreationDateTime)
            .WithAddress(address => address
                .WithCountry(AddressCountry)
                .WithCity(AddressCity)
                .WithStreet(AddressStreet)
                .WithNumber(AddressNumber)
            )
            .WithItem(item => item
              .WithSku(Sku)
              .WithUnitPrice(UnitPrice)
              .WithQuantity(Quantity)
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
            .WithId(Id)
            .WithBuyerId(BuyerId)
            .WithCreationDateTime(CreationDateTime)
            .WithAddress(address => address
                .WithCountry(AddressCountry)
                .WithCity(AddressCity)
                .WithStreet(AddressStreet)
                .WithNumber(AddressNumber)
            )
            .WithItem(item => item
              .WithSku(Sku)
              .WithUnitPrice(UnitPrice)
              .WithQuantity(Quantity)
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
            .WithId(Id)
            .WithBuyerId(BuyerId)
            .WithCreationDateTime(CreationDateTime)
            .WithAddress(address => address
                .WithCountry(AddressCountry)
                .WithCity(AddressCity)
                .WithStreet(AddressStreet)
                .WithNumber(AddressNumber)
            )
            .WithItem(item => item
              .WithSku(Sku)
              .WithUnitPrice(UnitPrice)
              .WithQuantity(Quantity)
            )
            .WithItem(item => item
              .WithSku("prod02")
              .WithUnitPrice(UnitPrice + 1)
              .WithQuantity(Quantity + 2)
            )
            .Build();
        await Given.AssumeOrderInRepository(existingOrder);

        var requestUrl = GetOrderUrl(existingOrder.Id);
        var response = await Given.Server.CreateClient().GetAsync<GetOrderByIdQueryApiResponse>(requestUrl);

        response.Items.Should().HaveCount(2);
        AssertOrderByIdApiResponse(response, existingOrder);
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsCorrectOrderWhenThereMultipleOrders()
    {
        var orderToBeReturned = new OrderBuilder()
            .WithId(Id)
            .WithBuyerId(BuyerId)
            .WithCreationDateTime(CreationDateTime)
            .WithAddress(address => address
                .WithCountry(AddressCountry)
                .WithCity(AddressCity)
                .WithStreet(AddressStreet)
                .WithNumber(AddressNumber)
            )
            .WithItem(item => item
              .WithSku(Sku)
              .WithUnitPrice(UnitPrice)
              .WithQuantity(Quantity)
            )
            .Build();

        var orderToBeIgnored = new OrderBuilder()
            .WithId(Guid.NewGuid())
            .WithBuyerId(Guid.NewGuid())
            .WithCreationDateTime(CreationDateTime.AddHours(1))
            .WithAddress(address => address
                .WithCountry("UK")
                .WithCity("London")
                .WithStreet("Downing Street")
                .WithNumber("10")
            )
            .WithItem(item => item
              .WithSku("prod02")
              .WithUnitPrice(UnitPrice + 1)
              .WithQuantity(Quantity + 1)
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
        actual.BuyerId.Should().Be(expected.BuyerId);
        actual.CreationDateTimeUtc.Should().Be(expected.CreationDateTime.UtcValue);
        actual.Status.Should().NotBeNull();
        actual.Status.Id.Should().Be(expected.Status.Id);
        actual.Status.Name.Should().Be(expected.Status.Name);
        actual.Address.Should().NotBeNull();
        actual.Address.Country.Should().Be(expected.Address.Country);
        actual.Address.City.Should().Be(expected.Address.City);
        actual.Address.Street.Should().Be(expected.Address.Street);
        actual.Address.Number.Should().Be(expected.Address.Number);


        actual.Items.Should().NotBeNull().And.HaveCount(expected.Items.Count);
        foreach (var item in actual.Items)
        {
            var corresponding = actual.Items.FirstOrDefault(x => x.Sku == item.Sku);
            corresponding.Should().NotBeNull();
            item.UnitPrice.Should().Be(corresponding!.UnitPrice);
            item.Quantity.Should().Be(corresponding!.Quantity);
        }
    }

    private static string GetOrderUrl(Guid id) => $"api/orders/{id}";
}
