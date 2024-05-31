using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace LibraryManagemetApi.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    internal class BookNotReservedException : Exception
    {
        public BookNotReservedException()
        {
        }

        public BookNotReservedException(string? message) : base(message)
        {
        }

        public BookNotReservedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BookNotReservedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}