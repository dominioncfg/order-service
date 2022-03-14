namespace OrderService.Consumer.Features.Order;

class OrderShippedIntegrationEventConsumer : IConsumer<OrderShippedIntegrationEvent>
{
    private readonly IOrderProjectionRepository _repository;

    public OrderShippedIntegrationEventConsumer(IOrderProjectionRepository repository)
    {
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<OrderShippedIntegrationEvent> context)
    {
        var order = await _repository.GetByIdOrDefaultAsync(context.Message.Id, default);
        if (order is null)
            throw new Exception("Order not found");
        order.Apply(context.Message);
        await _repository.UpdateAsync(order, default);

    }
}
