namespace OrderService.Domain.Orders;

[Serializable]
public class OrderItemsWithDifferentPricesDomainException : DomainException
{
    public OrderItemsWithDifferentPricesDomainException() { }
    public OrderItemsWithDifferentPricesDomainException(string message) : base(message) { }
    public OrderItemsWithDifferentPricesDomainException(string message, Exception inner) : base(message, inner) { }
    protected OrderItemsWithDifferentPricesDomainException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
