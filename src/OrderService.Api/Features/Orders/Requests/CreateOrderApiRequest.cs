using System;
using System.Collections.Generic;

namespace OrderService.Api.Features.Orders;

public record CreateOrderApiRequest
{
    public Guid Id { get; init; }
    public IEnumerable<CreateOrderItemApiRequest> Items { get; init; } = Array.Empty<CreateOrderItemApiRequest>();
}

public record CreateOrderItemApiRequest
{
    public string Sku { get; init; } = string.Empty;
    public decimal Quantity { get; init; }
}
