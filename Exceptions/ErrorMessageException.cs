using System;

namespace TeslaCamTheater.Exceptions
{
    /// <summary>
    /// Thrown when the exception's error message should be displayed to the user via the UI.
    /// </summary>
    [Serializable]
    public class ErrorMessageException : Exception
    {
        public ErrorMessageException() { }
        public ErrorMessageException(string message) : base(message) { }
        public ErrorMessageException(string message, Exception inner) : base(message, inner) { }
        protected ErrorMessageException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
