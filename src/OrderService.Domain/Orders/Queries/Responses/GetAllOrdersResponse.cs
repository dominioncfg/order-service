using System;
using System.Collections.Generic;

namespace OrderService.Domain.Orders;

public record GetAllOrdersResponse
{
    public IEnumerable<GetAllOrdersOrderResponse> Orders { get; init; } = Array.Empty<GetAllOrdersOrderResponse>();
}

public record GetAllOrdersOrderResponse
{
    public Guid Id { get; init; }

    public IEnumerable<GetAllOrdersOrderItemResponse> Items { get; init; } = Array.Empty<GetAllOrdersOrderItemResponse>();
}

public record GetAllOrdersOrderItemResponse
{
    public string Sku { get; init; } = string.Empty;
    public decimal Quantity { get; init; }
}
