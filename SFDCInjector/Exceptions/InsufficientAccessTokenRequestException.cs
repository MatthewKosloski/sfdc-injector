using System;

namespace SFDCInjector.Exceptions
{
    /// <summary>
    /// Thrown when not enough information is 
    /// known to make a request for an access token.
    /// </summary>
    public class InsufficientAccessTokenRequestException : Exception
    {
        public InsufficientAccessTokenRequestException()
        {
        }

        public InsufficientAccessTokenRequestException(string message)
            : base(message)
        {
        }

        public InsufficientAccessTokenRequestException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}