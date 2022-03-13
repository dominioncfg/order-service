namespace OrderService.Application.Features.Orders;

public record ShipOrderCommandHandler : IRequestHandler<ShipOrderCommand>
{
    private readonly IOrdersRepository _ordersRepository;

    public ShipOrderCommandHandler(IOrdersRepository ordersRepository)
    {
        _ordersRepository = ordersRepository;
    }

    public async Task<Unit> Handle(ShipOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await GetOrderAsync(request.Id, cancellationToken);
        order.MarkAsShipped();
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
