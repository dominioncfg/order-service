namespace OrderService.Domain.Orders;

[Serializable]
public class InvalidOrderBuyerIdDomainException : DomainException
{
    public InvalidOrderBuyerIdDomainException() { }
    public InvalidOrderBuyerIdDomainException(string message) : base(message) { }
    public InvalidOrderBuyerIdDomainException(string message, Exception inner) : base(message, inner) { }
    protected InvalidOrderBuyerIdDomainException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
