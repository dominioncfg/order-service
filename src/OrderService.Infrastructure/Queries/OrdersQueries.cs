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
        const string getByIdSql = @$"
        SELECT 
            id
        FROM  core.orders 
        WHERE id = @OrderId;
        SELECT 
            sku,
            quantity
        FROM core.order_items
        WHERE order_id = @OrderId;";
        var queryParams = new { OrderId = orderId };
        var response = await _dbQuery.QueryMultipleAsync<GetOrderByIdResponse, GetOrderByIdOrderItemResponse>(getByIdSql, queryParams, cancellationToken);

        if (!response.Item1.Any())
            return null;

        return response.Item1.First() with
        {
            Items = response.Item2,
        };
    }
    public async Task<GetAllOrdersResponse> GetAllOrdersAsync(CancellationToken cancellationToken)
    {
        var getByIdSql = @$"
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

        (IEnumerable<GetAllOrdersOrderQueryDto> AllOrders,
         IEnumerable<GetAllOrdersOrderItemQueryDto> AllOrderItems) = await _dbQuery
            .QueryMultipleAsync<GetAllOrdersOrderQueryDto, GetAllOrdersOrderItemQueryDto>(getByIdSql, null, cancellationToken);

        if (!AllOrders.Any())
            return new GetAllOrdersResponse() { Orders = new List<GetAllOrdersOrderResponse>() };

        var ordersToBeReturned = new List<GetAllOrdersOrderResponse>();

        foreach (var orderDto in AllOrders)
        {
            var orderItems = AllOrderItems
                    .Where(x => x.OrderId == orderDto.Id)
                    .ToArray();

            ordersToBeReturned.Add(new GetAllOrdersOrderResponse()
            {
                Id = orderDto.Id,
                BuyerId = orderDto.BuyerId,
                CreationDateTimeUtc = orderDto.CreationDateTime,
                Status = new GetAllOrdersOrderStatusResponse()
                {
                    Id = orderDto.StatusId,
                    Name = orderDto.StatusName,
                },
                Address= new GetAllOrdersOrderAddressResponse()
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
            });
        }

        return new GetAllOrdersResponse()
        {
            Orders = ordersToBeReturned.ToArray(),
        };
    }
}
