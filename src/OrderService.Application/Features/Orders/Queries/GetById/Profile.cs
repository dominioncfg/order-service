using AutoMapper;
using OrderService.Domain.Orders;

namespace OrderService.Application.Features.Orders.Queries.GetById
{
    public class GetOrderByIdQueryProfile : Profile
    {
        public GetOrderByIdQueryProfile()
        {
            CreateMap<GetOrderByIdResponse, GetOrderByIdQueryResponse>();
            CreateMap<GetOrderByIdOrderItemResponse, GetOrderByIdOrderItemQueryResponse>();
        }
    }
}
