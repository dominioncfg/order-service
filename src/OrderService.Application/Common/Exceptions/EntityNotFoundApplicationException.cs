using System;

namespace OrderService.Application.Common.Exceptions;

public class EntityNotFoundApplicationException : OrderServiceApplicationException
{
    public EntityNotFoundApplicationException() { }
    public EntityNotFoundApplicationException(string message) : base(message) { }
    public EntityNotFoundApplicationException(string message, Exception inner) : base(message, inner) { }
    protected EntityNotFoundApplicationException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
