using System;

namespace SkyRoute.Exceptions
{
    public class SeatsUnavailableException : Exception
    {
        public SeatsUnavailableException(string message) : base(message) { }

        public SeatsUnavailableException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}