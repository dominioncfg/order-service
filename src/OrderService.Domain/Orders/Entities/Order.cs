namespace OrderService.Domain.Orders;

public class Order : AggregateRoot<Guid>
{
    public Guid BuyerId { get; }
    public OrderCreationDate CreationDateTime { get; }
    public OrderStatus Status { get; private set; }
    public OrderAddress Address { get; }
    private readonly List<OrderItem> items = new();

    public IReadOnlyList<OrderItem> Items => items.ToList();

#nullable disable
    protected Order() { }
#nullable enable

    public Order(CreateOrderArgs args) : base(args.Id)
    {
        var id = args.Id;
        var buyerId = args.BuyerId;
        var orderItemsArgs = args.Items;
        var creationDate = args.CreationDateTimeUtc;
        var addressArgs = args.Address;

        if (id == default)
            throw new InvalidOrderIdDomainException("Order Id has a default value");

        if (buyerId == default)
            throw new InvalidOrderBuyerIdDomainException("Buyer is required");

        if (addressArgs == null)
            throw new OrderWithoutAdressDomainException("You are trying to create an order without corresponding address");

        if (orderItemsArgs is null || !orderItemsArgs.Any())
            throw new OrderWithNoItemsException($"Order '{id}' has no items");

        var orderItems = GetGroupedOrderItemsBySku(orderItemsArgs);

        BuyerId = buyerId;
        Status = OrderStatus.Submitted;
        CreationDateTime = OrderCreationDate.FromUtc(creationDate);
        Address = new(addressArgs.Country, addressArgs.City, addressArgs.Street, addressArgs.Number);

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
            var total = 0;
            var firstUnitPrice = orderItemGroup.First().UnitPrice;
            foreach (var item in orderItemGroup)
            {
                if (item.Quantity < 0)
                    throw new InvalidQuantityDomainException($"{item.Quantity} in order '{sku}' is invalid.");

                if (item.UnitPrice != firstUnitPrice)
                    throw new OrderItemsWithDifferentPricesDomainException("There are two items with the same sku but with different prices.");

                total += item.Quantity;
            }

            var orderItem = new OrderItem(Guid.NewGuid(), sku, firstUnitPrice, total);
            resultItems.Add(orderItem);
        }
        return resultItems;
    }

    public void MarkAsPaid()
    {
        if(!Status.CanChangeTo(OrderStatus.Paid))
            throw new OrderCannotBePaidDomainException($"Order with status '{Status.Name}' cannot be paid.");

        Status = OrderStatus.Paid;
        AddDomainEvent(OrderPaidDomainEvent.FromOrder(this));
    }
    public void MarkAsShipped() 
    {
        if (!Status.CanChangeTo(OrderStatus.Shipped))
            throw new OrderCannotBeShippedDomainException($"Order with status '{Status.Name}' cannot be shipped.");
        
        Status = OrderStatus.Shipped;
        AddDomainEvent(OrderShippedDomainEvent.FromOrder(this));
    }

    public void Cancel() 
    {
        if (!Status.CanChangeTo(OrderStatus.Cancelled))
            throw new OrderCannotBeCancelledDomainException($"Order with status '{Status.Name}' cannot be canncelled.");
        
        Status = OrderStatus.Cancelled;
        AddDomainEvent(OrderCancelledDomainEvent.FromOrder(this));
    }
}
