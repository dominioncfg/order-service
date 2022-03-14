namespace OrderService.Consumer.Features.Order;

class OrderSubmitedIntegrationEventConsumer : IConsumer<OrderSubmitedIntegrationEvent>
{
    private readonly IOrderProjectionRepository _repository;

    public OrderSubmitedIntegrationEventConsumer(IOrderProjectionRepository repository)
    {
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<OrderSubmitedIntegrationEvent> context)
    {
        var order = Order.FromIntegrationEvent(context.Message);
        await _repository.AddAsync(order, default);
    }
}
