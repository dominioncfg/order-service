using System;

namespace OrderService.Infrastructure.Queries;
public partial class OrdersQueries
{
    public record GetAllOrdersOrderItemQueryDto
    {
        public Guid OrderId { get; init; }
        public string Sku { get; init; } = string.Empty;
        public decimal Quantity { get; init; }
    }
}