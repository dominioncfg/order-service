namespace OrderService.Domain.UnitTests.Orders
{
    public abstract class OrderAggregateTestsBase
    {
        public Order GivenDefaultOrder(Guid id)
        {
            var order = new OrderBuilder()
            .WithId(id)
            .WithBuyerId(Guid.NewGuid())
            .WithCreationDateTime(new DateTime(2020, 10, 02).At(03, 00))
            .WithAddress(config => config
                .WithCountry("Spain")
                .WithCity("Madrid")
                .WithStreet("Gran Via")
                .WithNumber("55")
            )
            .WithItem(config => config
                .WithSku("prod01")
                .WithUnitPrice(10)
                .WithQuantity(1)
            )
            .Build();
            return order;
        }

        public Order GivenDefaultCancelledOrder(Guid id)
        {
            var order = GivenDefaultOrder(id);
            order.Cancel();
            return order;
        }

        public Order GivenDefaultShippedOrder(Guid id)
        {
            var order = GivenDefaultOrder(id);
            order.MarkAsPaid();
            order.MarkAsShipped();
            return order;
        }

        public Order GivenDefaultPaidOrder(Guid id)
        {
            var order = GivenDefaultOrder(id);
            order.MarkAsPaid();
            return order;
        }
    }
}