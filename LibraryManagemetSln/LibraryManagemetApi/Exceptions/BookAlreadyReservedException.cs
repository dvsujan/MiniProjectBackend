using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace LibraryManagemetApi.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class BookAlreadyReservedException : Exception
    {
        public BookAlreadyReservedException()
        {
        }

        public BookAlreadyReservedException(string? message) : base(message)
        {
        }

        public BookAlreadyReservedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BookAlreadyReservedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}