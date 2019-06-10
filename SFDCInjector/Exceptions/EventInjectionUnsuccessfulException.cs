using System;

namespace SFDCInjector.Exceptions
{
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