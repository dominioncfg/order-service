using System;

namespace OrderService.Application.Common.Exceptions;

public class BadRequestApplicationException : OrderServiceApplicationException
{
    public BadRequestApplicationException() { }
    public BadRequestApplicationException(string message) : base(message) { }
    public BadRequestApplicationException(string message, Exception inner) : base(message, inner) { }
    protected BadRequestApplicationException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
