namespace OrderService.Domain.UnitTests.Orders;

//This is a good place to test pure domain logic, prefer test entire aggregates instead of independant classes
public class OrderAggregateOrderItemTests
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

    [Fact]
    public void OrderItemsWithSameSkuAreCombined()
    {
        var skuToBeCombined = "prod02";
        var skuToBeCombinedUnitPrice = 3.4m;
        var skuToBeCombinedSkuQuantity1 = 3;
        var skuToBeCombinedSkuQuantity2 = 4;

        var order = new OrderBuilder()
            .WithId(Id)
            .WithBuyerId(BuyerId)
            .WithCreationDateTime(CreationDateTime)
            .WithAddress(config => config
                .WithCountry(AddressCountry)
                .WithCity(AddressCity)
                .WithStreet(AddressStreet)
                .WithNumber(AddressNumber)
            )
            .WithItem(config => config
                .WithSku(Sku)
                .WithUnitPrice(UnitPrice)
                .WithQuantity(Quantity)
            )
            .WithItem(config => config
                .WithSku(skuToBeCombined)
                .WithUnitPrice(skuToBeCombinedUnitPrice)
                .WithQuantity(skuToBeCombinedSkuQuantity1)
            )
            .WithItem(config => config
                .WithSku(skuToBeCombined)
                .WithUnitPrice(skuToBeCombinedUnitPrice)
                .WithQuantity(skuToBeCombinedSkuQuantity2)
            )
            .Build();

        order.Id.Should().Be(Id);
        order.Items.Should().HaveCount(2);

        var differentSkuOrderItem = order.Items.FirstOrDefault(x => x.Sku.Value == Sku);
        differentSkuOrderItem.Should().NotBeNull();
        differentSkuOrderItem!.UnitPrice.Value.Should().Be(UnitPrice);
        differentSkuOrderItem!.Quantity.Value.Should().Be(Quantity);

        var combinedOrderItem = order.Items.FirstOrDefault(x => x.Sku.Value == skuToBeCombined);
        combinedOrderItem.Should().NotBeNull();
        combinedOrderItem!.UnitPrice.Value.Should().Be(skuToBeCombinedUnitPrice);
        combinedOrderItem!.Quantity.Value.Should().Be(skuToBeCombinedSkuQuantity1 + skuToBeCombinedSkuQuantity2);
    }

    [Fact]
    public void ThrowsExceptionWhenCreatingWithoutItems()
    {
        var order = new OrderBuilder()
            .WithId(Id)
            .WithBuyerId(BuyerId)
            .WithCreationDateTime(CreationDateTime)
            .WithAddress(config => config
                .WithCountry(AddressCountry)
                .WithCity(AddressCity)
                .WithStreet(AddressStreet)
                .WithNumber(AddressNumber)
            );

        var action = () => order.Build();

        action.Should().Throw<OrderWithNoItemsException>();
    }

    [Fact]
    public void ThrowsExceptionWhenNegativeQuantity()
    {
        int invalidQuantity = -1;
        var order = new OrderBuilder()
            .WithId(Id)
            .WithBuyerId(BuyerId)
            .WithCreationDateTime(CreationDateTime)
            .WithAddress(config => config
                .WithCountry(AddressCountry)
                .WithCity(AddressCity)
                .WithStreet(AddressStreet)
                .WithNumber(AddressNumber)
            )
            .WithItem(config => config
                .WithSku(Sku)
                .WithUnitPrice(UnitPrice)
                .WithQuantity(invalidQuantity)
            );

        var action = () => order.Build();

        action.Should().Throw<InvalidQuantityDomainException>();
    }

    [Fact]
    public void ThrowsExceptionWhenNegativePrice()
    {
        var invalidPrice = -10;
        var order = new OrderBuilder()
            .WithId(Id)
            .WithBuyerId(BuyerId)
            .WithCreationDateTime(CreationDateTime)
            .WithAddress(config => config
                .WithCountry(AddressCountry)
                .WithCity(AddressCity)
                .WithStreet(AddressStreet)
                .WithNumber(AddressNumber)
            )
            .WithItem(config => config
                .WithSku(Sku)
                .WithUnitPrice(invalidPrice)
                .WithQuantity(Quantity)
            );

        var action = () => order.Build();

        action.Should().Throw<InvalidPriceDomainException>();
    }

    [Fact]
    public void ThrowsExceptionWhenEmptySku()
    {
        var invalidSku = string.Empty;
        var order = new OrderBuilder()
             .WithId(Id)
             .WithBuyerId(BuyerId)
             .WithCreationDateTime(CreationDateTime)
             .WithAddress(config => config
                 .WithCountry(AddressCountry)
                 .WithCity(AddressCity)
                 .WithStreet(AddressStreet)
                 .WithNumber(AddressNumber)
             )
             .WithItem(config => config
                 .WithSku(invalidSku)
                 .WithUnitPrice(UnitPrice)
                 .WithQuantity(Quantity)
             );

        var action = () => order.Build();

        action.Should().Throw<InvalidSkuDomainException>();
    }

    [Fact]
    public void FailToCombineOrderItemsWhenUnitPriceAreDifferents()
    {
        var differentUnitPrice = UnitPrice + 1;

        var order = new OrderBuilder()
             .WithId(Id)
             .WithBuyerId(BuyerId)
             .WithCreationDateTime(CreationDateTime)
             .WithAddress(config => config
                 .WithCountry(AddressCountry)
                 .WithCity(AddressCity)
                 .WithStreet(AddressStreet)
                 .WithNumber(AddressNumber)
             )
             .WithItem(config => config
                 .WithSku(Sku)
                 .WithUnitPrice(UnitPrice)
                 .WithQuantity(Quantity)
             )
             .WithItem(config => config
                 .WithSku(Sku)
                 .WithUnitPrice(differentUnitPrice)
                 .WithQuantity(Quantity)
             );
       
        var action = () => order.Build();

        action.Should().Throw<OrderItemsWithDifferentPricesDomainException>();
    }
}
