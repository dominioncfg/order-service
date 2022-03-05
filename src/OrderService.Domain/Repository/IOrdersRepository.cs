using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Domain;

public interface IOrdersRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken);

    Task<Order> GetByIdOrDefaultAsync(Guid id, CancellationToken cancellationToken);

    Task<List<Order>> GetAllAsync(CancellationToken cancellationToken);
}
