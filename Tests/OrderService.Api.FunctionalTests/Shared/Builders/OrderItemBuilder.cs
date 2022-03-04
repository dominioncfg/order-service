using OrderService.Domain;

namespace OrderService.Api.FunctionalTests.Shared
{
    public class OrderItemBuilder
    {
        private string sku;
        private decimal quantity;

        public OrderItemBuilder WithSku(string sku)
        {
            this.sku = sku;
            return this;
        }

        public OrderItemBuilder WithQuantity(decimal quantity)
        {
            this.quantity = quantity;
            return this;
        }

        public OrderItem Build() => new(sku, quantity);
    }
}
