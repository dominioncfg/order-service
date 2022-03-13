namespace OrderService.Contracts.Events.Integration;

//OrderService.Contracts could be published as an nuget for other microservices to consume
public class OrderCanceledIntegrationEvent
{
    public Guid Id { get; }

    public OrderCanceledIntegrationEvent(Guid id)
    {
        Id = id;
    }
}
