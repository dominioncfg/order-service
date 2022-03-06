using AutoMapper;
using MediatR;
using OrderService.Application.Common.Exceptions;
using OrderService.Domain.Orders;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Application.Features.Orders;

public record GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, GetOrderByIdQueryResponse>
{
    private readonly IMapper _mapper;
    private readonly IOrdersQueries _ordersQuery;

    public GetOrderByIdQueryHandler(IMapper mapper, IOrdersQueries ordersQuery)
    {
        _mapper = mapper;
        _ordersQuery = ordersQuery;
    }

    public async Task<GetOrderByIdQueryResponse> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var rawOrder = await _ordersQuery.GetOrderIdOrDefaultAsync(request.Id, cancellationToken);

        if (rawOrder is null)
            throw new EntityNotFoundApplicationException($"Order with id {request.Id} was not found.");

        var response = _mapper.Map<GetOrderByIdQueryResponse>(rawOrder);
        return response;
    }
}
