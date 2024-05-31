using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace LibraryManagemetApi.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class UserNotActiveException : Exception
    {
        public UserNotActiveException()
        {
        }

        public UserNotActiveException(string? message) : base(message)
        {
        }

        public UserNotActiveException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UserNotActiveException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}