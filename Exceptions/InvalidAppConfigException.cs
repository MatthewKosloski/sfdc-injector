using System;

namespace SFDCInjector.Exceptions
{
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