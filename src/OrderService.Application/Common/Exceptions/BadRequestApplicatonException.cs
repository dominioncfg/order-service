using System;

namespace OrderService.Application.Common.Exceptions
{
    public class BadRequestApplicatonException : Exception
    {
        public BadRequestApplicatonException() { }
        public BadRequestApplicatonException(string message) : base(message) { }
        public BadRequestApplicatonException(string message, Exception inner) : base(message, inner) { }
        protected BadRequestApplicatonException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    } 
}
