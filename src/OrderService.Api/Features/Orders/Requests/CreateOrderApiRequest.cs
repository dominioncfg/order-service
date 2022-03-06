using System;
using System.Collections.Generic;

namespace OrderService.Api.Features.Orders;

public record CreateOrderApiRequest
{
    public Guid Id { get; init; }
    public Dictionary<string, decimal> Items { get; init; } = new Dictionary<string, decimal>();
}
