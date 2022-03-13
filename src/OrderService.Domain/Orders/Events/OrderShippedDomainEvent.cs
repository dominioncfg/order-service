namespace OrderService.Domain.Orders;

public record OrderShippedDomainEvent(Guid OrderId) : IDomainEvent 
{
    public static OrderShippedDomainEvent FromOrder(Order o) => new(o.Id);
}