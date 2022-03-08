using OrderService.Domain.Seedwork;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Domain.Orders;

public interface IOrdersRepository: IAggregateRepository<Order,Guid>
{

    Task<Order?> GetByIdOrDefaultAsync(Guid id, CancellationToken cancellationToken);
}
