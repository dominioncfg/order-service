using OrderService.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderService.Domain.Orders;

public class Order : AggregateRoot<Guid>
{
    private readonly List<OrderItem> items = new();

    public IReadOnlyList<OrderItem> Items => items.ToList();

    protected Order() { }

    public Order(Guid id, IEnumerable<CreateOrderItemArgs> orderItemsArgs) : base(id)
    {
        if (id == default)
            throw new InvalidOrderIdException("Order Id has a default value");

        if (orderItemsArgs is null || !orderItemsArgs.Any())
            throw new OrderWithNoItemsException($"Order '{id}' has no items");

        var orderItems = GetGroupedOrderItemsBySku(orderItemsArgs);

        foreach (var orderItem in orderItems)
            AddOrderItem(orderItem);

        AddDomainEvent(OrderCreatedDomainEvent.FromOrder(this));
    }

    private void AddOrderItem(OrderItem order) => items.Add(order);

    private static List<OrderItem> GetGroupedOrderItemsBySku(IEnumerable<CreateOrderItemArgs> orderItems)
    {
        var itemsGrouped = orderItems.GroupBy(x => x.Sku);
        var resultItems = new List<OrderItem>();
        foreach (var orderItemGroup in itemsGrouped)
        {
            var sku = orderItemGroup.Key;
            var total = 0m;
            foreach (var item in orderItemGroup)
            {
                if (item.Quantity < 0)
                    throw new InvalidQuantityDomainException($"{item.Quantity} in order '{sku}' is invalid.");

                total += item.Quantity;
            }
            var orderItem = new OrderItem(sku, total);
            resultItems.Add(orderItem);
        }
        return resultItems;
    }
}
