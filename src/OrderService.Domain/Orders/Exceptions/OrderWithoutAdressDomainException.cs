namespace OrderService.Domain.Orders;

[Serializable]
public class OrderWithoutAdressDomainException : DomainException
{
    public OrderWithoutAdressDomainException() { }
    public OrderWithoutAdressDomainException(string message) : base(message) { }
    public OrderWithoutAdressDomainException(string message, Exception inner) : base(message, inner) { }
    protected OrderWithoutAdressDomainException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
