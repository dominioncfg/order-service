using MassTransit;
namespace OrderService.Application.Features.Orders;

public class OrderPaidPublisher : INotificationHandler<OrderPaidDomainEvent>
{
    //TODO: We could abstract MassTransit if we wanted to:
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderPaidPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(OrderPaidDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var integrationEvent = new OrderPaidIntegrationEvent(domainEvent.OrderId);
        await _publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}
