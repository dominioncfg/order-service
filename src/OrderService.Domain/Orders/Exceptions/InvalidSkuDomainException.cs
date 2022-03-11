namespace OrderService.Domain.Orders;

[Serializable]
public class InvalidSkuDomainException : DomainException
{
    public InvalidSkuDomainException() { }
    public InvalidSkuDomainException(string message) : base(message) { }
    public InvalidSkuDomainException(string message, Exception inner) : base(message, inner) { }
    protected InvalidSkuDomainException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}