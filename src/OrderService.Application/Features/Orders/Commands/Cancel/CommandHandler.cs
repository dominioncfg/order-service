namespace OrderService.Application.Features.Orders;

public record CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand>
{
    private readonly IOrdersRepository _ordersRepository;

    public CancelOrderCommandHandler(IOrdersRepository ordersRepository)
    {
        _ordersRepository = ordersRepository;
    }

    public async Task<Unit> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await GetOrderAsync(request.Id, cancellationToken);
        order.Cancel();
        await _ordersRepository.UpdateAsync(order, cancellationToken);
        return Unit.Value;
    }

    private async Task<Order> GetOrderAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var existingOrder = await _ordersRepository.GetByIdOrDefaultAsync(orderId, cancellationToken);

        if (existingOrder is null)
            throw new EntityNotFoundApplicationException($"Order with id {orderId} dont exist.");

        return existingOrder;
    }
}
