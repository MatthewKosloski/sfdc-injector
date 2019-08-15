using System;

namespace SFDCInjector.Exceptions
{

    /// <summary>
    /// Thrown when Salesforce sends back a response 
    /// indicating that the event has not been received.
    /// </summary>
    public class EventInjectionUnsuccessfulException : Exception
    {
        public EventInjectionUnsuccessfulException()
        {
        }

        public EventInjectionUnsuccessfulException(string message)
            : base(message)
        {
        }

        public EventInjectionUnsuccessfulException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}