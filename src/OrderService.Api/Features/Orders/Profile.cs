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
        CreateMap<GetAllOrdersOrderStatusQueryResponse, GetAllOrdersOrderStatusQueryApiResponse>();
        CreateMap<GetAllOrdersOrderAddressQueryResponse, GetAllOrdersOrderAddressQueryApiResponse>();
        CreateMap<GetAllOrdersOrderItemQueryResponse, GetAllOrdersOrderItemQueryApiResponse>();
    }

    private void MapGetById()
    {
        CreateMap<GetOrderByIdQueryResponse, GetOrderByIdQueryApiResponse>();
        CreateMap<GetOrderByIdOrderStatusQueryResponse, GetOrderByIdOrderStatusQueryApiResponse>();
        CreateMap<GetOrderByIdOrderAddressQueryResponse, GetOrderByIdOrderAddressQueryApiResponse>();
        CreateMap<GetOrderByIdOrderItemQueryResponse, GetOrderByIdOrderItemQueryApiResponse>();
    }

    private void MapCreate()
    {
        CreateMap<CreateOrderApiRequest, CreateOrderCommand>();
        CreateMap<CreateOrderAddressApiRequest, CreateOrderCommandAddress>();
        CreateMap<CreateOrderItemApiRequest, CreateOrderCommandItem>();
    }
}
