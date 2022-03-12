namespace OrderService.Application.Common.Exceptions;

[Serializable]
public class OrderServiceApplicationException : Exception
{
    public OrderServiceApplicationException() { }
    public OrderServiceApplicationException(string message) : base(message) { }
    public OrderServiceApplicationException(string message, Exception inner) : base(message, inner) { }
    protected OrderServiceApplicationException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
