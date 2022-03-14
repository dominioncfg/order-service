namespace OrderService.Consumer.Features.Order;

class OrderPaidIntegrationEventConsumer : IConsumer<OrderPaidIntegrationEvent>
{
    private readonly IOrderProjectionRepository _repository;

    public OrderPaidIntegrationEventConsumer(IOrderProjectionRepository repository)
    {
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<OrderPaidIntegrationEvent> context)
    {
        var order = await _repository.GetByIdOrDefaultAsync(context.Message.Id, default);
        if (order is null)
            throw new Exception("Order not found");
        order.Apply(context.Message);
        await _repository.UpdateAsync(order, default);

    }
}
