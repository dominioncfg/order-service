namespace OrderService.Domain.Orders;

[Serializable]
public class InvalidOrderAddressDomainException : DomainException
{
    public InvalidOrderAddressDomainException() { }
    public InvalidOrderAddressDomainException(string message) : base(message) { }
    public InvalidOrderAddressDomainException(string message, Exception inner) : base(message, inner) { }
    protected InvalidOrderAddressDomainException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
