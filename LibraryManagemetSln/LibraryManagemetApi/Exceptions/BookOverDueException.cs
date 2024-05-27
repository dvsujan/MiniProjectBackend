using System.Runtime.Serialization;

namespace LibraryManagemetApi.Exceptions
{
    [Serializable]
    internal class BookOverDueException : Exception
    {
        public BookOverDueException()
        {
        }

        public BookOverDueException(string? message) : base(message)
        {
        }

        public BookOverDueException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BookOverDueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}