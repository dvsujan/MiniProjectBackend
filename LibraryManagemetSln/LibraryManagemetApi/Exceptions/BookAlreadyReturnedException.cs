using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace LibraryManagemetApi.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    internal class BookAlreadyReturnedException : Exception
    {
        public BookAlreadyReturnedException()
        {
        }

        public BookAlreadyReturnedException(string? message) : base(message)
        {
        }

        public BookAlreadyReturnedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BookAlreadyReturnedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}