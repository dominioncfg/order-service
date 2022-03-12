namespace OrderService.Application.Features.Orders;

public class GetOrderByIdQueryProfile : Profile
{
    public GetOrderByIdQueryProfile()
    {
        CreateMap<GetOrderByIdResponse, GetOrderByIdQueryResponse>();
        CreateMap<GetOrderByIdOrderStatusResponse, GetOrderByIdOrderStatusQueryResponse>();
        CreateMap<GetOrderByIdOrderAddressResponse, GetOrderByIdOrderAddressQueryResponse>();
        CreateMap<GetOrderByIdOrderItemResponse, GetOrderByIdOrderItemQueryResponse>();
    }
}
