using OrderService.Api.Features.Orders;
using System;
using System.Collections.Generic;

namespace OrderService.Api.FunctionalTests.Features.Orders
{
    public class CreateOrderRequestBuilder
    {
        private Guid id;
        private readonly Dictionary<string, decimal> items = new();

        public CreateOrderRequestBuilder WithId(Guid id)
        {
            this.id = id;
            return this;
        }

        public CreateOrderRequestBuilder WithItem(string productSku, int quantity)
        {
            items.Add(productSku, quantity);
            return this;
        }

        public CreateOrderRequest Build() => new() { Id = id, Items = items };
    }
}
