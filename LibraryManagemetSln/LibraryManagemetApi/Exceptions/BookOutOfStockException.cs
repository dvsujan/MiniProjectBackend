using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace LibraryManagemetApi.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class BookOutOfStockException : Exception
    {
        public BookOutOfStockException()
        {
        }

        public BookOutOfStockException(string? message) : base(message)
        {
        }

        public BookOutOfStockException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BookOutOfStockException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}