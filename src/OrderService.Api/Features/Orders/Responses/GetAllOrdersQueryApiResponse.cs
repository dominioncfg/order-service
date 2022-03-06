using System;
using System.Collections.Generic;

namespace OrderService.Api.Features.Orders;

public record GetAllOrdersQueryApiResponse
{
    public IEnumerable<GetAllOrdersOrderQueryApiResponse> Orders { get; init; } = Array.Empty<GetAllOrdersOrderQueryApiResponse>();
}

public record GetAllOrdersOrderQueryApiResponse
{
    public Guid Id { get; init; }

    public IEnumerable<GetAllOrdersOrderItemQueryApiResponse> Items { get; init; } = Array.Empty<GetAllOrdersOrderItemQueryApiResponse>();
}

public record GetAllOrdersOrderItemQueryApiResponse
{
    public string Sku { get; init; } = string.Empty;
    public decimal Quantity { get; init; }
}

