using System;

namespace BizTrak.Application.Exceptions;

public class InvalidStateChangeException : Exception
{

        public InvalidStateChangeException()
        {
        }

        public InvalidStateChangeException(string message)
            : base(message)
        {
        }

        public InvalidStateChangeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
}
