using System;

namespace SFDCInjector.Exceptions
{
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