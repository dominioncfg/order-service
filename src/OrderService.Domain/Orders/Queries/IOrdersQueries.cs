using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Domain.Orders;

public interface IOrdersQueries
{
    public Task<GetOrderByIdResponse?> GetOrderIdOrDefaultAsync(Guid orderId, CancellationToken cancellationToken);
}
