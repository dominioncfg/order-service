using OrderService.Domain.Seedwork;
using System;

namespace OrderService.Domain.Events
{
    public record OrderCreatedDomainEvent : IDomainEvent
    {
        public Guid OrderId { get; init; }
        public OrderItemDto[] Items { get; init; } = Array.Empty<OrderItemDto>();
    }

    public record OrderItemDto(string Sku, decimal Quantity);
}
