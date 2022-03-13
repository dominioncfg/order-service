namespace OrderService.Domain.Orders;

[Serializable]
public class OrderCannotBeCancelledDomainException : DomainException
{
    public OrderCannotBeCancelledDomainException() { }
    public OrderCannotBeCancelledDomainException(string message) : base(message) { }
    public OrderCannotBeCancelledDomainException(string message, Exception inner) : base(message, inner) { }
    protected OrderCannotBeCancelledDomainException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
