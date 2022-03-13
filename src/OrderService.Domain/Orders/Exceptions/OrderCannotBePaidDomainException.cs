namespace OrderService.Domain.Orders;

[Serializable]
public class OrderCannotBePaidDomainException : DomainException
{
    public OrderCannotBePaidDomainException() { }
    public OrderCannotBePaidDomainException(string message) : base(message) { }
    public OrderCannotBePaidDomainException(string message, Exception inner) : base(message, inner) { }
    protected OrderCannotBePaidDomainException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
