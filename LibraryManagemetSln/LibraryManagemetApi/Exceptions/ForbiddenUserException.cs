using System.Runtime.Serialization;

namespace LibraryManagemetApi.Exceptions
{
    [Serializable]
    internal class ForbiddenUserException : Exception
    {
        public ForbiddenUserException()
        {
        }

        public ForbiddenUserException(string? message) : base(message)
        {
        }

        public ForbiddenUserException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ForbiddenUserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}