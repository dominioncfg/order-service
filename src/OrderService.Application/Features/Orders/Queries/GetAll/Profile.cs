using AutoMapper;
using OrderService.Domain.Orders;

namespace OrderService.Application.Features.Orders;

public class GetAllOrdersQueryProfile : Profile
{
    public GetAllOrdersQueryProfile()
    {
        CreateMap<GetAllOrdersResponse, GetAllOrdersQueryResponse>();
        CreateMap<GetAllOrdersOrderResponse, GetAllOrdersOrderQueryResponse>();
        CreateMap<GetAllOrdersOrderItemResponse, GetAllOrdersOrderItemQueryResponse>();
    }
}
