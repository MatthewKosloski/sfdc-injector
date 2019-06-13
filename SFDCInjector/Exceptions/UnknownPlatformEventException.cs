using System;

namespace SFDCInjector.Exceptions
{
    /// <summary>
    /// Thrown when the event class name supplied to
    /// the event creator does not resolve to a known type.
    /// </summary>
    public class UnknownPlatformEventException : Exception
    {
        public UnknownPlatformEventException()
        {
        }

        public UnknownPlatformEventException(string message)
            : base(message)
        {
        }

        public UnknownPlatformEventException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}