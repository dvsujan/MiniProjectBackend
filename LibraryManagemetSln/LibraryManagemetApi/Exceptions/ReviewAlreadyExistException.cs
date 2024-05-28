﻿using System.Runtime.Serialization;

namespace LibraryManagemetApi.Exceptions
{
    [Serializable]
    internal class ReviewAlreadyExistException : Exception
    {
        public ReviewAlreadyExistException()
        {
        }

        public ReviewAlreadyExistException(string? message) : base(message)
        {
        }

        public ReviewAlreadyExistException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ReviewAlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}