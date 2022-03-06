using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.DbQueries;
public class DbConnectionManagerBase
{
    protected readonly DbConnection Connection;
    private readonly bool CloseConnection;

    public DbConnectionManagerBase(DbConnection connection, bool closeConnection = true)
    {
        this.Connection = connection;
        this.CloseConnection = closeConnection;
    }

    protected async Task ExecuteInConnectionAsync(Func<DbConnection, Task> queryCallback, CancellationToken cancellationToken)
    {
        var connectionOpened = await CheckConnection(cancellationToken);
        try
        {
            await queryCallback(Connection);
        }
        finally
        {
            CloseIf(connectionOpened);
        }
    }

    protected async Task<bool> CheckConnection(CancellationToken cancellationToken)
    {
        if (this.Connection.State != ConnectionState.Closed)
        {
            return false;
        }

        await this.Connection.OpenAsync(cancellationToken);

        return true;
    }

    protected void CloseIf(bool connectionOpened)
    {
        if (connectionOpened && this.CloseConnection)
        {
            this.Connection.Close();
        }
    }
}
