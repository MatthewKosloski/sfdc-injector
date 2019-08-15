using System;

namespace SFDCInjector.Exceptions
{
    /// <summary>
    /// Thrown when not enough information is 
    /// known to inject an event into Salesforce.
    /// </summary>
    public class InsufficientEventInjectionException : Exception
    {
        public InsufficientEventInjectionException()
        {
        }

        public InsufficientEventInjectionException(string message)
            : base(message)
        {
        }

        public InsufficientEventInjectionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}