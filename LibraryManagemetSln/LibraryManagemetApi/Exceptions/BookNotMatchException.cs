using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace LibraryManagemetApi.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    internal class BookNotMatchException : Exception
    {
        public BookNotMatchException()
        {
        }

        public BookNotMatchException(string? message) : base(message)
        {
        }

        public BookNotMatchException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BookNotMatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}