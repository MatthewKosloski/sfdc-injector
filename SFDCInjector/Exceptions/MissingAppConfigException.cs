using System;

namespace SFDCInjector.Exceptions
{
    /// <summary>
    /// Thrown when there are missing cli authentication arguments 
    /// and there is no App.config file to fallback on.
    /// </summary>
    public class MissingAppConfigException : Exception
    {
        public MissingAppConfigException()
        {
        }

        public MissingAppConfigException(string message)
            : base(message)
        {
        }

        public MissingAppConfigException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}