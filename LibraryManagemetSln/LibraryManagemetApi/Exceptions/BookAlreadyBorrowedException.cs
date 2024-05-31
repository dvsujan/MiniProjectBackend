using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace LibraryManagemetApi.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class BookAlreadyBorrowedException : Exception
    {
        public BookAlreadyBorrowedException()
        {
        }

        public BookAlreadyBorrowedException(string? message) : base(message)
        {
        }

        public BookAlreadyBorrowedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}