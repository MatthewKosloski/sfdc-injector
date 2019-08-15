using System;

namespace SFDCInjector.Exceptions
{
    /// <summary>
    /// Thrown when an incomplete event is trying to 
    /// be injected into Salesforce.
    /// </summary>
    public class InvalidPlatformEventException : Exception
    {
        public InvalidPlatformEventException()
        {
        }

        public InvalidPlatformEventException(string message)
            : base(message)
        {
        }

        public InvalidPlatformEventException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}