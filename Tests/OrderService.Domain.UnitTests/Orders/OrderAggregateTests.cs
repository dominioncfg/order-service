namespace OrderService.Domain.UnitTests.Orders;

public class OrderAggregateTests
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
    public void CanCreateOrderWithSingleItem()
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
            )
            .WithItem(config => config
                .WithSku(Sku)
                .WithUnitPrice(UnitPrice)
                .WithQuantity(Quantity)
            )
            .Build();

        order.Id.Should().Be(Id);
        order.BuyerId.Should().Be(BuyerId);
        order.CreationDateTime.UtcValue.Should().Be(CreationDateTime);

        order.Address.Should().NotBeNull();
        order.Address.Country.Should().Be(AddressCountry);
        order.Address.City.Should().Be(AddressCity);
        order.Address.Street.Should().Be(AddressStreet);
        order.Address.Number.Should().Be(AddressNumber);

        order.Items.Should().HaveCount(1);
        var orderItem = order.Items.Single();
        orderItem.Id.Should().NotBeEmpty();
        orderItem.Sku.Value.Should().Be(Sku);
        orderItem.UnitPrice.PriceInEuros.Should().Be(UnitPrice);
        orderItem.Quantity.Value.Should().Be(Quantity);
    }

    [Fact]
    public void OrderCreatedEventIsRegistered()
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
            )
            .WithItem(config => config
                .WithSku(Sku)
                .WithUnitPrice(UnitPrice)
                .WithQuantity(Quantity)
            )
            .Build();

        order.Id.Should().Be(Id);
        order.DomainEvents.Should().HaveCount(1);
        var createdEvents = order.DomainEvents.OfType<OrderCreatedDomainEvent>().ToList();
        createdEvents.Should().HaveCount(1);
        var createdEvent = createdEvents.Single();

        createdEvent.OrderId.Should().Be(Id);

        createdEvent.BuyerId.Should().Be(BuyerId);
        createdEvent.CreationDateTime.UtcValue.Should().Be(CreationDateTime);

        createdEvent.Address.Should().NotBeNull();
        createdEvent.Address.Country.Should().Be(AddressCountry);
        createdEvent.Address.City.Should().Be(AddressCity);
        createdEvent.Address.Street.Should().Be(AddressStreet);
        createdEvent.Address.Number.Should().Be(AddressNumber);

        createdEvent.Items.Should().HaveCount(1);
        var eventOrderItem = createdEvent.Items.Single();

        eventOrderItem.Id.Should().Be(order.Items.Single().Id);
        eventOrderItem.Sku.Should().Be(Sku);
        eventOrderItem.UnitPrice.Should().Be(UnitPrice);
        eventOrderItem.Quantity.Should().Be(Quantity);
    }

    [Fact]
    public void ThrowsExceptionWhenIdIsInvalid()
    {
        var order = new OrderBuilder()
            .WithId(default)
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
            );

        var action = () => order.Build();

        action.Should().Throw<InvalidOrderIdDomainException>();
    }

    [Fact]
    public void ThrowsExceptionWhenBuyerIdIsInvalid()
    {
        var order = new OrderBuilder()
            .WithId(Id)
            .WithBuyerId(default)
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
            );

        var action = () => order.Build();

        action.Should().Throw<InvalidOrderBuyerIdDomainException>();
    }

    [Fact]
    public void ThrowsExceptionWhenCreationDateIsInvalid()
    {
        var order = new OrderBuilder()
            .WithId(Id)
            .WithBuyerId(BuyerId)
            .WithCreationDateTime(default)
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
            );

        var action = () => order.Build();

        action.Should().Throw<InvalidOrderCreationDateTimeDomainException>();
    }

    [Fact]
    public void ThrowsExceptionWhenOrderHasNoAddress()
    {
        var order = new OrderBuilder()
            .WithId(Id)
            .WithBuyerId(BuyerId)
            .WithCreationDateTime(CreationDateTime)
            .WithItem(config => config
                .WithSku(Sku)
                .WithUnitPrice(UnitPrice)
                .WithQuantity(Quantity)
            );

        var action = () => order.Build();

        action.Should().Throw<OrderWithoutAdressDomainException>();
    }

    [Fact]
    public void ThrowsExceptionWhenCountryAddressIsInvalid()
    {
        var order = new OrderBuilder()
            .WithId(Id)
            .WithBuyerId(BuyerId)
            .WithCreationDateTime(CreationDateTime)
            .WithAddress(config => config
                .WithCountry(string.Empty)
                .WithCity(AddressCity)
                .WithStreet(AddressStreet)
                .WithNumber(AddressNumber)
            )
            .WithItem(config => config
                .WithSku(Sku)
                .WithUnitPrice(UnitPrice)
                .WithQuantity(Quantity)
            );

        var action = () => order.Build();

        action.Should().Throw<InvalidOrderAddressDomainException>();
    }

    [Fact]
    public void ThrowsExceptionWhenCityAddressIsInvalid()
    {
        var order = new OrderBuilder()
            .WithId(Id)
            .WithBuyerId(BuyerId)
            .WithCreationDateTime(CreationDateTime)
            .WithAddress(config => config
                .WithCountry(AddressCountry)
                .WithCity(string.Empty)
                .WithStreet(AddressStreet)
                .WithNumber(AddressNumber)
            )
            .WithItem(config => config
                .WithSku(Sku)
                .WithUnitPrice(UnitPrice)
                .WithQuantity(Quantity)
            );

        var action = () => order.Build();

        action.Should().Throw<InvalidOrderAddressDomainException>();
    }

    [Fact]
    public void ThrowsExceptionWhenStreetAddressIsInvalid()
    {
        var order = new OrderBuilder()
            .WithId(Id)
            .WithBuyerId(BuyerId)
            .WithCreationDateTime(CreationDateTime)
            .WithAddress(config => config
                .WithCountry(AddressCountry)
                .WithCity(AddressCity)
                .WithStreet(string.Empty)
                .WithNumber(AddressNumber)
            )
            .WithItem(config => config
                .WithSku(Sku)
                .WithUnitPrice(UnitPrice)
                .WithQuantity(Quantity)
            );

        var action = () => order.Build();

        action.Should().Throw<InvalidOrderAddressDomainException>();
    }

    [Fact]
    public void ThrowsExceptionWhenAddressNumberIsInvalid()
    {
        var order = new OrderBuilder()
            .WithId(Id)
            .WithBuyerId(BuyerId)
            .WithCreationDateTime(CreationDateTime)
            .WithAddress(config => config
                .WithCountry(AddressCountry)
                .WithCity(AddressCity)
                .WithStreet(AddressStreet)
                .WithNumber(string.Empty)
            )
            .WithItem(config => config
                .WithSku(Sku)
                .WithUnitPrice(UnitPrice)
                .WithQuantity(Quantity)
            );

        var action = () => order.Build();

        action.Should().Throw<InvalidOrderAddressDomainException>();
    }
}
