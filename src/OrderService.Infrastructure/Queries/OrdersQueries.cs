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
        var queryParams = new { OrderId = orderId };
        var sql = @$"
        SELECT 
            {nameof(Order.Id).ToPostgreSQLQueryIdentifier()}
        FROM  {EntityFrameworkConfigurationConstants.MainSchema.ToPostgreSQLQueryIdentifier()}.{EntityFrameworkConfigurationConstants.Orders.ToPostgreSQLQueryIdentifier()} 
        WHERE {nameof(Order.Id).ToPostgreSQLQueryIdentifier()} = @{nameof(queryParams.OrderId)};
        SELECT 
            {nameof(OrderItem.Sku).ToPostgreSQLQueryIdentifier()},
            {nameof(OrderItem.Quantity).ToPostgreSQLQueryIdentifier()}
        FROM {EntityFrameworkConfigurationConstants.MainSchema.ToPostgreSQLQueryIdentifier()}.{EntityFrameworkConfigurationConstants.OrdersItems.ToPostgreSQLQueryIdentifier()} 
        WHERE {"OrderId".ToPostgreSQLQueryIdentifier()} = @{nameof(queryParams.OrderId)};";

        var response = await _dbQuery.QueryMultipleAsync<GetOrderByIdResponse, GetOrderByIdOrderItemResponse>(sql, queryParams, cancellationToken);

        if (response.Item1 is null)
            return null;

        return response.Item1 with
        {
            Items = response.Item2,
        };
    }
}


public static class SqlExtensions
{
    public static string ToPostgreSQLQueryIdentifier(this string originalName) => $"\"{originalName}\"";
}
