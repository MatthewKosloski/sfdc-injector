using System;

namespace SFDCInjector.Exceptions
{
    /// <summary>
    /// Thrown when the Index property of the CommandLineArgumentIndexAttribute
    /// is out of range.
    /// </summary>
    public class InvalidCommandLineArgumentIndexException : Exception
    {
        public InvalidCommandLineArgumentIndexException()
        {
        }

        public InvalidCommandLineArgumentIndexException(string message)
            : base(message)
        {
        }

        public InvalidCommandLineArgumentIndexException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}