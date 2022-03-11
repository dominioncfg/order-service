namespace OrderService.Domain.Orders;

[Serializable]
public class InvalidPriceDomainException : DomainException
{
    public InvalidPriceDomainException() { }
    public InvalidPriceDomainException(string message) : base(message) { }
    public InvalidPriceDomainException(string message, Exception inner) : base(message, inner) { }
    protected InvalidPriceDomainException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
