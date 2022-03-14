namespace OrderService.Contracts.Events.Integration;

//OrderService.Contracts could be published as an nuget for other microservices to consume
public class OrderCancelledIntegrationEvent
{
    public Guid Id { get; }

    public OrderCancelledIntegrationEvent(Guid id)
    {
        Id = id;
    }
}
