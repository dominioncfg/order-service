﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.DbQueries;

public class OrdersDbQuery : DbConnectionManagerBase, IOrdersDbQuery
{
    public OrdersDbQuery(DbConnection connection, bool closeConnection = true) : base(connection, closeConnection) { }

    public async Task<IEnumerable<T>> QueryListAsync<T>(string sql, object? parameters = null, CancellationToken cancellationToken = default)
    {
        IEnumerable<T> response = Array.Empty<T>();
        await ExecuteInConnectionAsync(async connection =>
        {
            response = await connection.QueryAsync<T>(sql, parameters);
        }, cancellationToken);

        return response;
    }

    public async Task<(T1?, IEnumerable<T2>)> QueryMultipleAsync<T1, T2>(string sqlQuery, object? param = null, CancellationToken cancellationToken = default)
    {
        T1? item1 = default;
        IEnumerable<T2>? item2 = Array.Empty<T2>();

        await ExecuteInConnectionAsync(async connection =>
        {
            var queryResult = await Connection.QueryMultipleAsync(sqlQuery, param);
            (item1, item2) = (queryResult.Read<T1>().FirstOrDefault(), queryResult.Read<T2>());
        }, cancellationToken);

        return (item1 , item2);
    }
}
