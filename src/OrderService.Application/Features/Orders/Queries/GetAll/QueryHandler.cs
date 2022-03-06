using AutoMapper;
using MediatR;
using OrderService.Domain.Orders;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Application.Features.Orders;

public record GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, GetAllOrdersQueryResponse>
{
    private readonly IMapper _mapper;
    private readonly IOrdersQueries _ordersQuery;

    public GetAllOrdersQueryHandler(IMapper mapper, IOrdersQueries ordersQuery)
    {
        _mapper = mapper;
        _ordersQuery = ordersQuery;
    }

    public async Task<GetAllOrdersQueryResponse> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var rawOrders = await _ordersQuery.GetAllOrdersAsync(cancellationToken);

        var response = _mapper.Map<GetAllOrdersQueryResponse>(rawOrders);

        return response;
    }
}
