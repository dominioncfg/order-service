using MediatR;
using System;

namespace OrderService.Application.Features.Orders;

public record GetOrderByIdQuery : IRequest<GetOrderByIdQueryResponse>
{
    public Guid Id { get; init; }
}

