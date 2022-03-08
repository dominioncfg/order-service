using OrderService.Domain.Seedwork;
using System;

namespace OrderService.Domain.Orders;

[Serializable]
public class InvalidOrderIdException : DomainException
{
    public InvalidOrderIdException() { }
    public InvalidOrderIdException(string message) : base(message) { }
    public InvalidOrderIdException(string message, Exception inner) : base(message, inner) { }
    protected InvalidOrderIdException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
