using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace LibraryManagemetApi.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    internal class ForbiddenBorrowException : Exception
    {
        public ForbiddenBorrowException()
        {
        }

        public ForbiddenBorrowException(string? message) : base(message)
        {
        }

        public ForbiddenBorrowException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ForbiddenBorrowException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}