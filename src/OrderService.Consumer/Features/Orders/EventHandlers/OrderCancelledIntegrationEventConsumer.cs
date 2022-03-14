namespace OrderService.Consumer.Features.Order;

class OrderCancelledIntegrationEventConsumer : IConsumer<OrderCancelledIntegrationEvent>
{
    private readonly IOrderProjectionRepository _repository;

    public OrderCancelledIntegrationEventConsumer(IOrderProjectionRepository repository)
    {
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<OrderCancelledIntegrationEvent> context)
    {
        await _repository.DeleteByIdAsync(context.Message.Id, default);
    }
}