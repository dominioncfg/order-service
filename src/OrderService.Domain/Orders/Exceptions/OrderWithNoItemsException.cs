using OrderService.Domain.Seedwork;
using System;

namespace OrderService.Domain.Orders;

[Serializable]
public class OrderWithNoItemsException : DomainException
{
    public OrderWithNoItemsException() { }
    public OrderWithNoItemsException(string message) : base(message) { }
    public OrderWithNoItemsException(string message, Exception inner) : base(message, inner) { }
    protected OrderWithNoItemsException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
