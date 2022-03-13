using OrderService.Contracts.Events.Integration;

namespace OrderService.Api.FunctionalTests.Features.Orders;

[Collection(nameof(TestServerFixtureCollection))]
public class WhenCreatingOrders
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

    public WhenCreatingOrders(TestServerFixture given)
    {
        Given = given ?? throw new Exception("Null Server");
    }

    [Fact]
    [ResetApplicationState]
    public async Task CanCreateOrderWithSingleItem()
    {
        Given.AssumeClockUtcNowAt(CreationDateTime);
        var request = new CreateOrderApiRequestBuilder()
            .WithId(Id)
            .WithBuyerId(BuyerId)
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

        await Given.Server.CreateClient().PostAndExpectCreatedAsync(PostCreateUrl(), request);

        var ordersInDb = await Given.GetAllOrdersInRepository();
        ordersInDb.Should().NotBeNull().And.HaveCount(1);
        AssertOrderFromRequest(ordersInDb.First(), request, CreationDateTime);
    }

    [Fact]
    [ResetApplicationState]
    public async Task CanCreateOrderWithTwoItems()
    {
        Given.AssumeClockUtcNowAt(CreationDateTime);
        var request = new CreateOrderApiRequestBuilder()
            .WithId(Id)
            .WithBuyerId(BuyerId)
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

        await Given.Server.CreateClient().PostAndExpectCreatedAsync(PostCreateUrl(), request);

        var ordersInDb = await Given.GetAllOrdersInRepository();
        ordersInDb.Should().NotBeNull().And.HaveCount(1);
        AssertOrderFromRequest(ordersInDb.First(), request, CreationDateTime);
    }

    [Fact]
    [ResetApplicationState]
    public async Task CanCreateOrderWhenAnotherOrderAlreadyExists()
    {
        Given.AssumeClockUtcNowAt(CreationDateTime);
        var request = new CreateOrderApiRequestBuilder()
            .WithId(Id)
            .WithBuyerId(BuyerId)
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
        var existingOrder = new OrderBuilder()
            .WithId(Guid.NewGuid())
            .WithBuyerId(Guid.NewGuid())
            .WithCreationDateTime(CreationDateTime.AddHours(-1))
            .WithAddress(address => address
                .WithCountry("Spain")
                .WithCity("Barcelone")
                .WithStreet("Diagonal")
                .WithNumber("20")
            )
            .WithItem(item => item
                .WithSku("product-sku-02")
                .WithUnitPrice(2)
                .WithQuantity(4)
            )
            .Build();
        await Given.AssumeOrderInRepository(existingOrder);

        await Given.Server.CreateClient().PostAndExpectCreatedAsync(PostCreateUrl(), request);

        var ordersInDb = await Given.GetAllOrdersInRepository();
        ordersInDb.Should().NotBeNull().And.HaveCount(2);
        var theOrder = ordersInDb.FirstOrDefault(x => x.Id == request.Id);
        theOrder.Should().NotBeNull();
        AssertOrderFromRequest(theOrder!, request, CreationDateTime);
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsBadRequestWhenIdAlreadyExists()
    {
        Given.AssumeClockUtcNowAt(CreationDateTime);
        var existingOrder = new OrderBuilder()
            .WithId(Id)
            .WithBuyerId(Guid.NewGuid())
            .WithCreationDateTime(CreationDateTime.AddHours(-1))
            .WithAddress(address => address
                .WithCountry("Spain")
                .WithCity("Barcelone")
                .WithStreet("Diagonal")
                .WithNumber("20")
            )
            .WithItem(item => item
                .WithSku("product-sku-02")
                .WithUnitPrice(2)
                .WithQuantity(4)
            )
            .Build();

        var request = new CreateOrderApiRequestBuilder()
            .WithId(Id)
            .WithBuyerId(BuyerId)
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

        await Given.Server.CreateClient().PostAndExpectBadRequestAsync(PostCreateUrl(), request);
    }

    [Fact]
    [ResetApplicationState]
    public async Task SendsEventWhenOrderIsCreated()
    {
        Given.AssumeClockUtcNowAt(CreationDateTime);
        var request = new CreateOrderApiRequestBuilder()
           .WithId(Id)
           .WithBuyerId(BuyerId)
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

        await Given.Server.CreateClient().PostAndExpectCreatedAsync(PostCreateUrl(), request);

        var ordersInDb = await Given.GetAllOrdersInRepository();

        var events = Given.GetPublishedEventsOfType<OrderSubmitedIntegrationEvent>();
        ordersInDb.Should().NotBeNull().And.HaveCount(1);
        events.Should().NotBeNull().And.HaveCount(1);
        AssertOrderFromRequest(ordersInDb.First(), request, CreationDateTime);
        AssertEventFromRequest(events.First(), request);
    }

    [Fact]
    [ResetApplicationState]
    public async Task OrderItemsWithSameSkuAreCombined()
    {
        Given.AssumeClockUtcNowAt(CreationDateTime);
        var skuToBeCombinedSkuQuantity1 = Quantity;
        var skuToBeCombinedSkuQuantity2 = Quantity + 2;
        var differentSku = "prod02";
        var differentUnitPrice = UnitPrice + 2;
        var differentSkuQuantity = Quantity + 4;

        var request = new CreateOrderApiRequestBuilder()
            .WithId(Id)
            .WithBuyerId(BuyerId)
            .WithAddress(address => address
                .WithCountry(AddressCountry)
                .WithCity(AddressCity)
                .WithStreet(AddressStreet)
                .WithNumber(AddressNumber)
            )
            .WithItem(item => item
                .WithSku(Sku)
                .WithUnitPrice(UnitPrice)
                .WithQuantity(skuToBeCombinedSkuQuantity1)
            )
            .WithItem(item => item
                .WithSku(Sku)
                .WithUnitPrice(UnitPrice)
                .WithQuantity(skuToBeCombinedSkuQuantity2)
            )
            .WithItem(item => item
                .WithSku(differentSku)
                .WithUnitPrice(differentUnitPrice)
                .WithQuantity(differentSkuQuantity)
            )
            .Build();

        await Given.Server.CreateClient().PostAndExpectCreatedAsync(PostCreateUrl(), request);

        var ordersInDb = await Given.GetAllOrdersInRepository();
        ordersInDb.Should().NotBeNull().And.HaveCount(1);
        var orderInDb = ordersInDb.First();

        orderInDb.Id.Should().Be(request.Id);
        orderInDb.CreationDateTime.UtcValue.Should().Be(CreationDateTime);
        orderInDb.Items.Should().HaveCount(2);

        var differentSkuOrderItem = orderInDb.Items.FirstOrDefault(x => x.Sku.Value == differentSku);
        differentSkuOrderItem.Should().NotBeNull();
        differentSkuOrderItem!.UnitPrice.PriceInEuros.Should().Be(differentUnitPrice);
        differentSkuOrderItem!.Quantity.Value.Should().Be(differentSkuQuantity);

        var combinedOrderItem = orderInDb.Items.FirstOrDefault(x => x.Sku.Value == Sku);
        combinedOrderItem.Should().NotBeNull();
        combinedOrderItem!.UnitPrice.PriceInEuros.Should().Be(UnitPrice);
        combinedOrderItem!.Quantity.Value.Should().Be(skuToBeCombinedSkuQuantity1 + skuToBeCombinedSkuQuantity2);
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsBadRequestWhenAnyOrderItemHasZeroQuantityEvenIfCouldBeCombined()
    {
        Given.AssumeClockUtcNowAt(CreationDateTime);
        var request = new CreateOrderApiRequestBuilder()
           .WithId(Id)
           .WithBuyerId(BuyerId)
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
               .WithSku(Sku)
               .WithUnitPrice(UnitPrice)
               .WithQuantity(0)
           )
           .Build();

        await Given.Server.CreateClient().PostAndExpectBadRequestAsync(PostCreateUrl(), request);
    }

    [Fact]
    [ResetApplicationState]
    public async Task DontSendEventWhenRequestIsInvalid()
    {
        Given.AssumeClockUtcNowAt(CreationDateTime);
        var request = ValidRequest().WithId(default).Build();

        await Given.Server.CreateClient().PostAndExpectBadRequestAsync(PostCreateUrl(), request);

        var events = Given.GetPublishedEventsOfType<OrderSubmitedIntegrationEvent>();
        events.Should().NotBeNull().And.BeEmpty();
    }

    #region Fluent Validation 
    // All of this tests are replicated from the OrderService.Application.UnitTests so we could leave just one to make sure FluentValidation is in place
    // But it also can be interesting to have integration tests with all validations.
    [Fact]
    [ResetApplicationState]
    public async Task ReturnsBadRequestWhenOrderIdIsNotValid()
    {
        Given.AssumeClockUtcNowAt(CreationDateTime);
        var request = ValidRequest()
            .WithId(default)
            .Build();
        await Given.Server.CreateClient().PostAndExpectBadRequestAsync(PostCreateUrl(), request);
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsBadRequestWhenBuyerIdIsNotValid()
    {
        Given.AssumeClockUtcNowAt(CreationDateTime);
        var request = ValidRequest()
            .WithBuyerId(default)
            .Build();
        await Given.Server.CreateClient().PostAndExpectBadRequestAsync(PostCreateUrl(), request);
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsBadRequestWhenOrderHasNoItems()
    {
        Given.AssumeClockUtcNowAt(CreationDateTime);
        var request = ValidRequest()
             .WithNoItems()
             .Build();

        await Given.Server.CreateClient().PostAndExpectBadRequestAsync(PostCreateUrl(), request);
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsBadRequestWhenOrderHasItemsWithNegativeQuantity()
    {
        Given.AssumeClockUtcNowAt(CreationDateTime);
        var request = ValidRequest()
            .WithItem(item => item
                .WithSku("prod02")
                .WithUnitPrice(UnitPrice)
                .WithQuantity(-2)
            )
            .Build();

        await Given.Server.CreateClient().PostAndExpectBadRequestAsync(PostCreateUrl(), request);
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsBadRequestWhenOrderHasItemsWithNegativeUnitPrice()
    {
        Given.AssumeClockUtcNowAt(CreationDateTime);
        var request = ValidRequest()
            .WithItem(item => item
                .WithSku("prod02")
                .WithUnitPrice(-1)
                .WithQuantity(Quantity)
            )
            .Build();

        await Given.Server.CreateClient().PostAndExpectBadRequestAsync(PostCreateUrl(), request);
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsBadRequestWhenOrderHasItemsWithZeroQuantity()
    {
        Given.AssumeClockUtcNowAt(CreationDateTime);
        var request = ValidRequest()
             .WithItem(item => item
                 .WithSku("prod02")
                 .WithUnitPrice(UnitPrice)
                 .WithQuantity(0)
             )
             .Build();

        await Given.Server.CreateClient().PostAndExpectBadRequestAsync(PostCreateUrl(), request);
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsBadRequestWhenOrderHasItemsWithZeroUnitPrice()
    {
        Given.AssumeClockUtcNowAt(CreationDateTime);
        var request = ValidRequest()
             .WithItem(item => item
                 .WithSku("prod02")
                 .WithUnitPrice(0)
                 .WithQuantity(Quantity)
             )
             .Build();

        await Given.Server.CreateClient().PostAndExpectBadRequestAsync(PostCreateUrl(), request);
    }

    [Fact]
    [ResetApplicationState]
    public async Task ReturnsBadRequestWhenOrderHasItemsWithEmptySKU()
    {
        Given.AssumeClockUtcNowAt(CreationDateTime);
        var request = ValidRequest()
             .WithItem(item => item
                 .WithSku(string.Empty)
                 .WithUnitPrice(UnitPrice)
                 .WithQuantity(Quantity)
             )
             .Build();

        await Given.Server.CreateClient().PostAndExpectBadRequestAsync(PostCreateUrl(), request);
    }
    #endregion

    private static void AssertOrderFromRequest(Order actual, CreateOrderApiRequest expected, DateTime ocurredAt)
    {
        actual.Should().NotBeNull();
        actual.Id.Should().Be(expected.Id);
        actual.BuyerId.Should().Be(expected.BuyerId);
        actual.Status.Should().Be(OrderStatus.Submitted);
        actual.CreationDateTime.UtcValue.Should().Be(ocurredAt);

        actual.Address.Should().NotBeNull();
        actual.Address.Country.Should().Be(expected.Address.Country);
        actual.Address.City.Should().Be(expected.Address.City);
        actual.Address.Street.Should().Be(expected.Address.Street);
        actual.Address.Number.Should().Be(expected.Address.Number);

        actual.Items.Should().NotBeNull().And.HaveCount(expected.Items.Count());
        foreach (var item in actual.Items)
        {
            var corresponding = actual.Items.FirstOrDefault(x => x.Sku == item.Sku);
            corresponding.Should().NotBeNull();
            item.Quantity.Should().Be(corresponding!.Quantity);
            item.UnitPrice.Should().Be(corresponding!.UnitPrice);
        }
    }

    private static void AssertEventFromRequest(OrderSubmitedIntegrationEvent actual, CreateOrderApiRequest expected)
    {
        actual.Should().NotBeNull();
        actual.Id.Should().Be(expected.Id);
        actual.BuyerId.Should().Be(expected.BuyerId);
        
        actual.Address.Should().NotBeNull();
        actual.Address.Country.Should().Be(expected.Address.Country);
        actual.Address.City.Should().Be(expected.Address.City);
        actual.Address.Street.Should().Be(expected.Address.Street);
        actual.Address.Number.Should().Be(expected.Address.Number);
        actual.Items.Should().NotBeNull().And.HaveCount(expected.Items.Count());
        foreach (var item in actual.Items)
        {
            var corresponding = actual.Items.FirstOrDefault(x => x.Sku == item.Sku);
            corresponding.Should().NotBeNull();
            item.Quantity.Should().Be(corresponding!.Quantity);
            item.UnitPrice.Should().Be(corresponding!.UnitPrice);
        }
    }
    private CreateOrderApiRequestBuilder ValidRequest()
    {
        return new CreateOrderApiRequestBuilder()
           .WithId(Id)
           .WithBuyerId(BuyerId)
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
           );
    }

    private static string PostCreateUrl() => "api/orders";
}
