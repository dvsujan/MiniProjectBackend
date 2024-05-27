using System.Runtime.Serialization;

namespace LibraryManagemetApi.Exceptions
{
    [Serializable]
    internal class BookAlreadyReservedException : Exception
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