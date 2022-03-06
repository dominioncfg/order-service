using MediatR;
using System;

namespace OrderService.Application.Features.Orders;

public record CreateOrderCommand : IRequest
{
    public Guid Id { get; init; }
    public CreateOrderCommandItem[] Items { get; init; } = Array.Empty<CreateOrderCommandItem>();
}

public record CreateOrderCommandItem
{
    public string Sku { get; init; } = string.Empty; 

    public decimal Quantity { get; init; }
}
