using System;

namespace OrderService.Infrastructure.Queries;
public partial class OrdersQueries
{
    public record GetAllOrdersOrderQueryDto
    {
        public Guid Id { get; init; }
    }
}