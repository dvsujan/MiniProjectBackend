using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace LibraryManagemetApi.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class ForbiddenCardException : Exception
    {
        public ForbiddenCardException()
        {
        }

        public ForbiddenCardException(string? message) : base(message)
        {
        }

        public ForbiddenCardException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ForbiddenCardException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}