﻿using System.Runtime.Serialization;

namespace LibraryManagemetApi.Exceptions
{
    [Serializable]
    internal class BookAlreadyExistsException : Exception
    {
        public BookAlreadyExistsException()
        {
        }

        public BookAlreadyExistsException(string? message) : base(message)
        {
        }

        public BookAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BookAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}