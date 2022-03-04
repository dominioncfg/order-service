using Microsoft.EntityFrameworkCore;
using OrderService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly OrdersDbContext _ordersDbContext;

        public OrdersRepository(OrdersDbContext ordersDbContext)
        {
            _ordersDbContext = ordersDbContext;
        }

        public async Task AddAsync(Order order, CancellationToken cancellationToken)
        {
            await _ordersDbContext.AddAsync(order, cancellationToken);
            //For more complex scenarios instead of using this we can call this from  a UnitOfWork
            await _ordersDbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<Order> GetByIdOrDefaultAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _ordersDbContext
                .Orders
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<List<Order>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _ordersDbContext
              .Orders
              .Include(x => x.Items)
              .ToListAsync(cancellationToken);
        } 

    }
}
