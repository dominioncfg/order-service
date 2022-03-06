using AutoMapper;
using OrderService.Application.Features.Orders;
using System.Linq;

namespace OrderService.Api.Features.Orders;

public class OrdersControllerProfile : Profile
{
    public OrdersControllerProfile()
    {
        MapGetAll();
        MapGetById();
        MapCreate();
    }

    private void MapGetAll()
    {
        CreateMap<GetAllOrdersQueryResponse, GetAllOrdersQueryApiResponse>();
        CreateMap<GetAllOrdersOrderQueryResponse, GetAllOrdersOrderQueryApiResponse>();
        CreateMap<GetAllOrdersOrderItemQueryResponse, GetAllOrdersOrderItemQueryApiResponse>();
    }

    private void MapGetById()
    {
        CreateMap<GetOrderByIdQueryResponse, GetOrderByIdQueryApiResponse>();
        CreateMap<GetOrderByIdOrderItemQueryResponse, GetOrderByIdOrderItemQueryApiResponse>();
    }

    private void MapCreate()
    {
        CreateMap<CreateOrderApiRequest, CreateOrderCommand>()
                    .ForMember(x => x.Items, opt => opt.MapFrom(x => x.Items.Select(y => new CreateOrderCommandItem()
                    {
                        Sku = y.Key,
                        Quantity = y.Value,
                    }).ToArray()));
    }
}
