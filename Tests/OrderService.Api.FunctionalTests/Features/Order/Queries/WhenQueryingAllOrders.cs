namespace OrderService.Api.FunctionalTests.Features.Orders;

[Collection(nameof(TestServerFixtureCollection))]
public class WhenQueryingAllOrders
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
            .WithQuantity(Quantity + 1)
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
            .Build(),
            new OrderBuilder()
            .WithId(Guid.NewGuid())
            .WithBuyerId(BuyerId)
            .WithCreationDateTime(CreationDateTime)
            .WithAddress(address => address
                .WithCountry(AddressCountry)
                .WithCity(AddressCity)
                .WithStreet(AddressStreet)
                .WithNumber(AddressNumber)
            )           
            .WithItem(item => item
              .WithSku("prod02")
              .WithUnitPrice(UnitPrice + 1)
              .WithQuantity(Quantity + 1)
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
              .WithQuantity(Quantity + 1)
            )
            .Build(),
            new OrderBuilder()
            .WithId(Guid.NewGuid())
            .WithBuyerId(BuyerId)
            .WithCreationDateTime(CreationDateTime)
            .WithAddress(address => address
                .WithCountry(AddressCountry)
                .WithCity(AddressCity)
                .WithStreet(AddressStreet)
                .WithNumber(AddressNumber)
            )
            .WithItem(item => item
              .WithSku("prod03")
              .WithUnitPrice(UnitPrice + 2)
              .WithQuantity(Quantity + 3)
            )
            .WithItem(item => item
              .WithSku("prod04")
              .WithUnitPrice(UnitPrice + 3)
              .WithQuantity(Quantity + 4)
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

    private static string GetOrdersUrl() => $"api/orders/";
}
