using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace LibraryManagemetApi.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class BookNotBorrowedException : Exception
    {
        public BookNotBorrowedException()
        {
        }

        public BookNotBorrowedException(string? message) : base(message)
        {
        }

        public BookNotBorrowedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BookNotBorrowedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}