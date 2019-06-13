using System;

namespace SFDCInjector.Exceptions
{
    /// <summary>
    /// Thrown when the event fields class name supplied to
    /// the event creator does not resolve to a known type.
    /// </summary>
    public class UnknownPlatformEventFieldsException : Exception
    {
        public UnknownPlatformEventFieldsException()
        {
        }

        public UnknownPlatformEventFieldsException(string message)
            : base(message)
        {
        }

        public UnknownPlatformEventFieldsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}