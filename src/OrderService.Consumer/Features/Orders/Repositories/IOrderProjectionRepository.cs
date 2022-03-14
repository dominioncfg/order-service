namespace OrderService.Consumer.Features.Order;

public interface IOrderProjectionRepository
{
    Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken);
    Task<Order?> GetByIdOrDefaultAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Order o, CancellationToken cancellationToken);
    Task UpdateAsync(Order o, CancellationToken cancellationToken);
    Task DeleteByIdAsync(Guid orderId, CancellationToken cancellationToken);
}
