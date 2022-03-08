using System;

namespace OrderService.Domain.Orders;

[Serializable]
public class InvalidQuantityDomainException : DomainException
{
    public InvalidQuantityDomainException() { }
    public InvalidQuantityDomainException(string message) : base(message) { }
    public InvalidQuantityDomainException(string message, Exception inner) : base(message, inner) { }
    protected InvalidQuantityDomainException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
