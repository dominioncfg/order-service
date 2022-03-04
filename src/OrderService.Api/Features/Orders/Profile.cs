using AutoMapper;
using OrderService.Application.Features.Orders;
using System.Linq;
using ApplicationNamespace = OrderService.Application.Features.Orders;
namespace OrderService.Api.Features.Orders
{
    public class OrdersControllerProfile : Profile
    {
        public OrdersControllerProfile()
        {
            CreateMap<CreateOrderRequest, ApplicationNamespace.CreateOrderCommand>()
                .ForMember(x => x.Items, opt => opt.MapFrom(x => x.Items.Select(y => new CreateOrderItem()
                {
                    Sku = y.Key,
                    Quantity = y.Value,
                }).ToArray()));

        }
    }
}
