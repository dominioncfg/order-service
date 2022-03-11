namespace OrderService.Domain.Orders;

[Serializable]
public class InvalidOrderCreationDateTimeDomainException : DomainException
{
    public InvalidOrderCreationDateTimeDomainException() { }
    public InvalidOrderCreationDateTimeDomainException(string message) : base(message) { }
    public InvalidOrderCreationDateTimeDomainException(string message, Exception inner) : base(message, inner) { }
    protected InvalidOrderCreationDateTimeDomainException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
