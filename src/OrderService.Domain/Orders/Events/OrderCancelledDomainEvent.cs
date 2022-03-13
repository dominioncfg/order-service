namespace OrderService.Domain.Orders;

public record OrderCancelledDomainEvent(Guid OrderId) : IDomainEvent 
{
    public static OrderCancelledDomainEvent FromOrder(Order o) => new(o.Id);
}