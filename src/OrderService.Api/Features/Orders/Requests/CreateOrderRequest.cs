using System;
using System.Collections.Generic;

namespace OrderService.Api.Features.Orders;

public class CreateOrderRequest
{
    public Guid Id { get; init; }
    public Dictionary<string, decimal> Items { get; init; } = new Dictionary<string, decimal>();
}
