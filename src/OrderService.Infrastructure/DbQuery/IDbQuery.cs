namespace OrderService.Infrastructure.DbQueries;

public interface IDbQuery
{
    Task<IEnumerable<T>> QueryListAsync<T>(string sql, object? parameters = null, CancellationToken cancellationToken = default);
    Task<(IEnumerable<T1>, IEnumerable<T2>)> QueryMultipleAsync<T1, T2>(string sqlQuery, object? param = null, CancellationToken cancellationToken = default);
}
