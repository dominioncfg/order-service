namespace OrderService.Domain.Orders;

[Serializable]
public class InvalidOrderIdDomainException : DomainException
{
    public InvalidOrderIdDomainException() { }
    public InvalidOrderIdDomainException(string message) : base(message) { }
    public InvalidOrderIdDomainException(string message, Exception inner) : base(message, inner) { }
    protected InvalidOrderIdDomainException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
