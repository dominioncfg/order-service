using MediatR;
using System;

namespace OrderService.Application.Features.Orders;

public record CreateOrderCommand : IRequest
{
    public Guid Id { get; init; }
    public CreateOrderItem[] Items { get; init; } = Array.Empty<CreateOrderItem>();
}

public record CreateOrderItem
{
    public string Sku { get; init; }

    public decimal Quantity { get; init; }
}
