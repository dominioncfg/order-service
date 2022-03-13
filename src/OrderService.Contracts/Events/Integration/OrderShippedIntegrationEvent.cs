namespace OrderService.Contracts.Events.Integration;

//OrderService.Contracts could be published as an nuget for other microservices to consume
public class OrderShippedIntegrationEvent
{
    public Guid Id { get; }

    public OrderShippedIntegrationEvent(Guid id)
    {
        Id = id;
    }
}
