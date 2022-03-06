using AutoMapper;
using OrderService.Application.Features.Orders;
using System.Linq;

namespace OrderService.Api.Features.Orders;

public class OrdersControllerProfile : Profile
{
    public OrdersControllerProfile()
    {
        CreateMap<CreateOrderApiRequest, CreateOrderCommand>()
            .ForMember(x => x.Items, opt => opt.MapFrom(x => x.Items.Select(y => new CreateOrderCommandItem()
            {
                Sku = y.Key,
                Quantity = y.Value,
            }).ToArray()));
        CreateMap<GetOrderByIdQueryResponse, GetOrderByIdQueryApiResponse>();
        CreateMap<GetOrderByIdOrderItemQueryResponse, GetOrderByIdOrderItemQueryApiResponse>();
    }
}
