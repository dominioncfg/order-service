using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.DbQueries;

public interface IDbQuery
{
    Task<IEnumerable<T>> QueryListAsync<T>(string sql, object? parameters = null, CancellationToken cancellationToken = default);
    Task<(T1?, IEnumerable<T2>)> QueryMultipleAsync<T1, T2>(string sqlQuery, object? param = null, CancellationToken cancellationToken = default);
}
