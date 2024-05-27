using System.Runtime.Serialization;

namespace LibraryManagemetApi.Exceptions
{
    [Serializable]
    internal class UserNotMatchException : Exception
    {
        public UserNotMatchException()
        {
        }

        public UserNotMatchException(string? message) : base(message)
        {
        }

        public UserNotMatchException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UserNotMatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}