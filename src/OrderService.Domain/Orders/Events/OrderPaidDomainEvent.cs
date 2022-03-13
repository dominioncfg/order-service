namespace OrderService.Domain.Orders;

public record OrderPaidDomainEvent(Guid OrderId) : IDomainEvent 
{
    public static OrderPaidDomainEvent FromOrder(Order o) => new(o.Id);
}