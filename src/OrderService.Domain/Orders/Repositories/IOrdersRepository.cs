namespace OrderService.Domain.Orders;

public interface IOrdersRepository: IAggregateRepository<Order,Guid>
{
    Task<Order?> GetByIdOrDefaultAsync(Guid id, CancellationToken cancellationToken);  
}
