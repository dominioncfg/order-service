using System;

namespace OrderService.Application.Common.Exceptions;

[Serializable]
public class OrderServiceApplicatonException : Exception
{
    public OrderServiceApplicatonException() { }
    public OrderServiceApplicatonException(string message) : base(message) { }
    public OrderServiceApplicatonException(string message, Exception inner) : base(message, inner) { }
    protected OrderServiceApplicatonException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
