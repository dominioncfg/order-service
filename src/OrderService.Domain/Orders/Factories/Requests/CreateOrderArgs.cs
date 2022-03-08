using System;
using System.Collections.Generic;

namespace OrderService.Domain.Orders;

public record CreateOrderArgs
{
    public Guid Id { get; init; }
    public IEnumerable<CreateOrderItemArgs> Items { get; init; } = Array.Empty<CreateOrderItemArgs>();

}
