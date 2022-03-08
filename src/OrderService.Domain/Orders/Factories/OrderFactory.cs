namespace OrderService.Domain.Orders;

public class OrderFactory
{
    public static Order Create(CreateOrderArgs args) => new (args.Id, args.Items);
}
