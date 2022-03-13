namespace OrderService.Domain.Orders;

[Serializable]
public class OrderCannotBeShippedDomainException : DomainException
{
    public OrderCannotBeShippedDomainException() { }
    public OrderCannotBeShippedDomainException(string message) : base(message) { }
    public OrderCannotBeShippedDomainException(string message, Exception inner) : base(message, inner) { }
    protected OrderCannotBeShippedDomainException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
