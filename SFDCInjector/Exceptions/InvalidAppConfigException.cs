using System;

namespace SFDCInjector.Exceptions
{
    /// <summary>
    /// Thrown when an App.config file exists, but it is missing one or more settings.
    /// </summary>
    public class InvalidAppConfigException : Exception
    {
        public InvalidAppConfigException()
        {
        }

        public InvalidAppConfigException(string message)
            : base(message)
        {
        }

        public InvalidAppConfigException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}