using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Domain.Orders;

public interface IOrdersRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken);

    Task<Order?> GetByIdOrDefaultAsync(Guid id, CancellationToken cancellationToken);
}
