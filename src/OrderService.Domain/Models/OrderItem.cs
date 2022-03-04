using System;

namespace OrderService.Domain
{
    public class OrderItem
    {
        public string Sku { get; }
        public decimal Quantity { get; }

        protected OrderItem() { }

        public OrderItem(string sku, decimal quantity)
        {
            Sku = sku;
            Quantity = quantity;
        }
    }
}
