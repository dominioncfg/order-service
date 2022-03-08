using FluentAssertions;
using OrderService.Domain.Orders;
using OrderService.Tests.Common.Builders;
using System;
using System.Linq;
using Xunit;

namespace OrderService.Domain.UnitTests.Orders;

//This is a good place to test pure domain logic, prefer test entire aggregates instead of independant classes
public class OrderAggregateTests
{
    [Fact]
    public void CanCreateOrderWithSingleItem()
    {
        var id = Guid.NewGuid();
        var sku = "prod01";
        var quantity = 1;

        var order = new OrderBuilder()
                .WithId(id)
                .WithItem(config=>config
                    .WithSku(sku)
                    .WithQuantity(quantity)
                )
                .Build();

        order.Id.Should().Be(id);
        order.Items.Should().HaveCount(1);
        var orderItem = order.Items.Single();
        orderItem.Sku.Should().Be(sku);
        orderItem.Quantity.Should().Be(quantity);
    }

    [Fact]
    public void OrderCreatedEventIsRegistered()
    {
        var id = Guid.NewGuid();
        var sku = "prod01";
        var quantity = 1;

        var order = new OrderBuilder()
                .WithId(id)
                .WithItem(config => config
                    .WithSku(sku)
                    .WithQuantity(quantity)
                )
                .Build();

        order.Id.Should().Be(id);
        order.DomainEvents.Should().HaveCount(1);
        var createdEvents = order.DomainEvents.OfType<OrderCreatedDomainEvent>().ToList();
        createdEvents.Should().HaveCount(1);
        var createdEvent = createdEvents.Single();
        createdEvent.OrderId.Should().Be(id);
        createdEvent.Items.Should().HaveCount(1);
        var eventOrderItem = createdEvent.Items.Single();
        eventOrderItem.Sku.Should().Be(sku);
        eventOrderItem.Quantity.Should().Be(quantity);
    }

    [Fact]
    public void ThrowsExcetpionWhenIdIsInvalid()
    {
        Guid id = default;
        var sku = "prod01";
        var quantity = 1;

        var order = new OrderBuilder()
                .WithId(id)
                .WithItem(config => config
                    .WithSku(sku)
                    .WithQuantity(quantity)
                );

        var action = () => order.Build();

        action.Should().Throw<InvalidOrderIdException>();
    }


    [Fact]
    public void ThrowsExcetpionWhenCreatingWithoutItems()
    {
        Guid id = Guid.NewGuid();

        var order = new OrderBuilder()
                .WithId(id);

        var action = () => order.Build();

        action.Should().Throw<OrderWithNoItemsException>();
    }

    [Fact]
    public void ThrowsExcetpionWhenNegativeQuantity()
    {
        Guid id = Guid.NewGuid();
        var sku = "prod01";
        var quantity = -1;

        var order = new OrderBuilder()
                .WithId(id)
                .WithItem(config => config
                    .WithSku(sku)
                    .WithQuantity(quantity)
                );

        var action = () => order.Build();

        action.Should().Throw<InvalidQuantityDomainException>();
    }

    [Fact]
    //Fails at OrderItem Level
    public void ThrowsExcetpionWhenEmptySku()
    {
        Guid id = Guid.NewGuid();
        var sku = "";
        var quantity = 1;

        var order = new OrderBuilder()
                .WithId(id)
                .WithItem(config => config
                    .WithSku(sku)
                    .WithQuantity(quantity)
                );

        var action = () => order.Build();

        action.Should().Throw<InvalidSkuDomainException>();
    }

    [Fact]
    public void OrderItems_With_Same_Sku_Are_Combined()
    {
        var id = Guid.NewGuid();
        var differentSku = "product-sku-02";
        var differentSkuQuantity = 2;
        var skuToBeCombined = "product-sku-01";
        var skuToBeCombinedSkuQuantity1 = 3;
        var skuToBeCombinedSkuQuantity2 = 4;
        
        var order = new OrderBuilder()
            .WithId(id)
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

        order.Id.Should().Be(id);
        order.Items.Should().HaveCount(2);

        var differentSkuOrderItem = order.Items.FirstOrDefault(x => x.Sku == differentSku);
        differentSkuOrderItem.Should().NotBeNull();
        differentSkuOrderItem!.Quantity.Should().Be(differentSkuQuantity);

        var combinedOrderItem = order.Items.FirstOrDefault(x => x.Sku == skuToBeCombined);
        combinedOrderItem.Should().NotBeNull();
        combinedOrderItem!.Quantity.Should().Be(skuToBeCombinedSkuQuantity1 + skuToBeCombinedSkuQuantity2);
    }
}
