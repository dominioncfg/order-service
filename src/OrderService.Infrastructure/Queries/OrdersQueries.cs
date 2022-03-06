using EFCore.NamingConventions.Internal;
using OrderService.Domain.Orders;
using OrderService.Infrastructure.DbQueries;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.Queries;
public class OrdersQueries : IOrdersQueries
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

        if (response.Item1 is null)
            return null;

        return response.Item1 with
        {
            Items = response.Item2,
        };
    }
}