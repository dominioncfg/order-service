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
        const string getByIdSql = @$"
        SELECT 
            id
        FROM  core.orders; 
        SELECT 
            order_id as ""orderId"",
            sku,
            quantity
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