using System;

namespace OrderService.Infrastructure.Queries;

public partial class OrdersQueries
{
    public record GetOrdersByIdOrderQueryDto
    {
        public Guid Id { get; init; }
        public Guid BuyerId { get; init; }
        public DateTime CreationDateTime { get; }
        public int StatusId { get; init; }
        public string StatusName { get; init; } = string.Empty;
        public string Country { get; } = string.Empty;
        public string City { get; } = string.Empty;
        public string Street { get; } = string.Empty;
        public string Number { get; } = string.Empty;
    }
    
    public record GetOrdersByIdOrderItemQueryDto
    {
        public Guid OrderId { get; init; }
        public string Sku { get; init; } = string.Empty;
        public decimal UnitPrice { get; init; }
        public int Quantity { get; init; }
    }
}