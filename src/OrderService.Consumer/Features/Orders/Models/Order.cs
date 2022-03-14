namespace OrderService.Consumer.Features.Order;

public class Order
{
    public Guid Id { get; }
    public Guid BuyerId { get; }
    public DateTime CreationDateTime { get; }
    public string StatusName { get; private set; } = string.Empty;
    public string FullAddress { get; } = string.Empty;
    public int TotalNumberOfItems { get; }
    public decimal TotalAmount { get; }

    private Order(Guid id, Guid buyerId, DateTime creationDateTime, string statusName, string fullAddress, int totalNumberOfItems, decimal totalAmount)
    {
        Id = id;
        BuyerId = buyerId;
        CreationDateTime = creationDateTime;
        StatusName = statusName;
        FullAddress = fullAddress;
        TotalNumberOfItems = totalNumberOfItems;
        TotalAmount = totalAmount;
    }

    public static Order FromIntegrationEvent(OrderSubmitedIntegrationEvent integrationEvent)
    {
        var fullAddress = $"{integrationEvent.Address.Street} {integrationEvent.Address.Number}, {integrationEvent.Address.City},{integrationEvent.Address.Country}";
        var totalItems = integrationEvent.Items.Select(x => x.Quantity).Sum();
        var totalPrice = integrationEvent.Items.Select(x => x.Quantity * x.UnitPrice).Sum();
        return new Order(integrationEvent.Id, integrationEvent.BuyerId, integrationEvent.CreationDateTimeUtc, "Submitted", fullAddress, totalItems, totalPrice);
    }

    public void Apply(OrderPaidIntegrationEvent _)
    {
        StatusName = "Paid";
    }

    public void Apply(OrderShippedIntegrationEvent _)
    {
        StatusName = "Shipped";
    }
}
