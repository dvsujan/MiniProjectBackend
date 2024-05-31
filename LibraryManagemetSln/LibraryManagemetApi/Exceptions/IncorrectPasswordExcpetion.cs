using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace LibraryManagemetApi.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class IncorrectPasswordExcpetion : Exception
    {
        public IncorrectPasswordExcpetion()
        {
        }

        public IncorrectPasswordExcpetion(string? message) : base(message)
        {
        }

        public IncorrectPasswordExcpetion(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected IncorrectPasswordExcpetion(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        public override string Message => "Passwords Does not match";
    }
}