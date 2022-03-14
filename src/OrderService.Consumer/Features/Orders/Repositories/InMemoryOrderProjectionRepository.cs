namespace OrderService.Consumer.Features.Order;

public class InMemoryOrderProjectionRepository : IOrderProjectionRepository
{
    private readonly List<Order> _orders;
    public InMemoryOrderProjectionRepository(List<Order> orders)
    {
        _orders = orders;
    }

    public Task AddAsync(Order o, CancellationToken cancellationToken)
    {
        _orders.Add(o);
        return Task.CompletedTask;
    }

    public Task DeleteByIdAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var orderIndex = _orders.FindIndex(x => x.Id == orderId);
        if (orderIndex >= 0)
            _orders.RemoveAt(orderIndex);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult<IEnumerable<Order>>(_orders.ToList().AsReadOnly().ToList());
    }

    public Task<Order?> GetByIdOrDefaultAsync(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_orders.FirstOrDefault(x => x.Id == id));
    }

    public Task UpdateAsync(Order o, CancellationToken cancellationToken)
    {
        var orderIndex = _orders.FindIndex(x => x.Id == o.Id);
        if (orderIndex >= 0)
            _orders[orderIndex] = o;
        return Task.CompletedTask;

    }   
}
