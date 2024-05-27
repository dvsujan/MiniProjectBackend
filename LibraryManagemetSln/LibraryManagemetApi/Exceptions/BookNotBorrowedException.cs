using System.Runtime.Serialization;

namespace LibraryManagemetApi.Exceptions
{
    [Serializable]
    internal class BookNotBorrowedException : Exception
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