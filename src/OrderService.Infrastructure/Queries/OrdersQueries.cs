using OrderService.Domain.Orders;
using OrderService.Infrastructure.DbQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.Queries;
public partial class OrdersQueries : IOrdersQueries
{
    private readonly IOrdersDbQuery _dbQuery;

    public OrdersQueries(IOrdersDbQuery dbQuery)
    {
        _dbQuery = dbQuery;
    }

    public async Task<GetOrderByIdResponse?> GetOrderIdOrDefaultAsync(Guid orderId, CancellationToken cancellationToken)
    {
        string getByIdSql = GetOrderByIdSqlQuery();
        var queryParams = new { OrderId = orderId };

        (var orders, var orderItems) = await _dbQuery
            .QueryMultipleAsync<GetOrdersByIdOrderQueryDto, GetOrdersByIdOrderItemQueryDto>
                (getByIdSql, queryParams, cancellationToken);

        if (!orders.Any())
            return null;

        var orderDto = orders.Single();
        return MapGetByIdInfraQueryResponse(orderDto,orderItems);
    }

    public async Task<GetAllOrdersResponse> GetAllOrdersAsync(CancellationToken cancellationToken)
    {
        string getAllOrdersSql = GetAllOrdersSqlQuery();

        (var allOrders, var allOrderItems) = await _dbQuery
            .QueryMultipleAsync<GetAllOrdersOrderQueryDto, GetAllOrdersOrderItemQueryDto>
                (getAllOrdersSql, null, cancellationToken);

        if (!allOrders.Any())
            return new GetAllOrdersResponse() { Orders = new List<GetAllOrdersOrderResponse>() };
       
        return new GetAllOrdersResponse()
        {
            Orders = MapGetAllOrderResponse(allOrders, allOrderItems),
        };
    }    

    private static string GetOrderByIdSqlQuery()
    {
        return @$"
        SELECT 
            id as {nameof(GetOrdersByIdOrderQueryDto.Id).ToSqlField()},
            buyer_id as {nameof(GetOrdersByIdOrderQueryDto.BuyerId).ToSqlField()},
            creation_date_time as {nameof(GetOrdersByIdOrderQueryDto.CreationDateTime).ToSqlField()},
            status_id as {nameof(GetOrdersByIdOrderQueryDto.StatusId).ToSqlField()},
            status_name as {nameof(GetOrdersByIdOrderQueryDto.StatusName).ToSqlField()},
            address_country as {nameof(GetOrdersByIdOrderQueryDto.Country).ToSqlField()},
            address_city as {nameof(GetOrdersByIdOrderQueryDto.City).ToSqlField()},
            address_street as {nameof(GetOrdersByIdOrderQueryDto.Street).ToSqlField()},
            address_number as {nameof(GetOrdersByIdOrderQueryDto.Number).ToSqlField()}
        FROM  core.orders
        WHERE id = @OrderId;
        SELECT 
            order_id as {nameof(GetOrdersByIdOrderItemQueryDto.OrderId).ToSqlField()},
            sku as {nameof(GetOrdersByIdOrderItemQueryDto.Sku).ToSqlField()},
            unit_price as {nameof(GetOrdersByIdOrderItemQueryDto.UnitPrice).ToSqlField()},
            quantity as {nameof(GetOrdersByIdOrderItemQueryDto.Quantity).ToSqlField()}
        FROM core.order_items
        WHERE order_id = @OrderId;";
    }

    private static GetOrderByIdResponse MapGetByIdInfraQueryResponse(GetOrdersByIdOrderQueryDto orderDto, IEnumerable<GetOrdersByIdOrderItemQueryDto> orderItems)
    {
        return new GetOrderByIdResponse()
        {
            Id = orderDto.Id,
            BuyerId = orderDto.BuyerId,
            CreationDateTimeUtc = orderDto.CreationDateTime,
            Status = new GetOrderByIdOrderStatusResponse()
            {
                Id = orderDto.StatusId,
                Name = orderDto.StatusName,
            },
            Address = new GetOrderByIdOrderAddressResponse()
            {
                Country = orderDto.Country,
                City = orderDto.City,
                Street = orderDto.Street,
                Number = orderDto.Number,
            },
            Items = orderItems.Select(x => new GetOrderByIdOrderItemResponse()
            {
                Sku = x.Sku,
                Quantity = x.Quantity,
            })
        };
    }

    private static string GetAllOrdersSqlQuery()
    {
        return @$"
        SELECT 
            id as {nameof(GetAllOrdersOrderQueryDto.Id).ToSqlField()},
            buyer_id as {nameof(GetAllOrdersOrderQueryDto.BuyerId).ToSqlField()},
            creation_date_time as {nameof(GetAllOrdersOrderQueryDto.CreationDateTime).ToSqlField()},
            status_id as {nameof(GetAllOrdersOrderQueryDto.StatusId).ToSqlField()},
            status_name as {nameof(GetAllOrdersOrderQueryDto.StatusName).ToSqlField()},
            address_country as {nameof(GetAllOrdersOrderQueryDto.Country).ToSqlField()},
            address_city as {nameof(GetAllOrdersOrderQueryDto.City).ToSqlField()},
            address_street as {nameof(GetAllOrdersOrderQueryDto.Street).ToSqlField()},
            address_number as {nameof(GetAllOrdersOrderQueryDto.Number).ToSqlField()}
        FROM  core.orders;         
        SELECT 
            order_id as {nameof(GetAllOrdersOrderItemQueryDto.OrderId).ToSqlField()},
            sku as {nameof(GetAllOrdersOrderItemQueryDto.Sku).ToSqlField()},
            unit_price as {nameof(GetAllOrdersOrderItemQueryDto.UnitPrice).ToSqlField()},
            quantity as {nameof(GetAllOrdersOrderItemQueryDto.Quantity).ToSqlField()}
        FROM core.order_items;";
    }

    private static List<GetAllOrdersOrderResponse> MapGetAllOrderResponse(IEnumerable<GetAllOrdersOrderQueryDto> allOrders, IEnumerable<GetAllOrdersOrderItemQueryDto> allOrderItems)
    {
        var ordersToBeReturned = new List<GetAllOrdersOrderResponse>();
        foreach (var orderDto in allOrders)
        {
            var orderItems = allOrderItems
                    .Where(x => x.OrderId == orderDto.Id)
                    .ToArray();
            var order = MapSingleGetAllOrdersInfraQueryResponse(orderDto, orderItems);
            ordersToBeReturned.Add(order);
        }

        return ordersToBeReturned;
    }

    private static GetAllOrdersOrderResponse MapSingleGetAllOrdersInfraQueryResponse(GetAllOrdersOrderQueryDto orderDto, GetAllOrdersOrderItemQueryDto[] orderItems)
    {
        return new GetAllOrdersOrderResponse()
        {
            Id = orderDto.Id,
            BuyerId = orderDto.BuyerId,
            CreationDateTimeUtc = orderDto.CreationDateTime,
            Status = new GetAllOrdersOrderStatusResponse()
            {
                Id = orderDto.StatusId,
                Name = orderDto.StatusName,
            },
            Address = new GetAllOrdersOrderAddressResponse()
            {
                Country = orderDto.Country,
                City = orderDto.City,
                Street = orderDto.Street,
                Number = orderDto.Number,
            },
            Items = orderItems.Select(x => new GetAllOrdersOrderItemResponse()
            {
                Sku = x.Sku,
                Quantity = x.Quantity,
            })
        };
    }
}
