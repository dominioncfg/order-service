namespace OrderService.Contracts.Events.Integration;

//OrderService.Contracts could be published as an nuget for other microservices to consume
public class OrderPaidIntegrationEvent
{
    public Guid Id { get; }

    public OrderPaidIntegrationEvent(Guid id)
    {
        Id = id;
    }
}
