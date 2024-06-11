namespace LibraryManagemetApi.Exceptions
{
    [Serializable]
    internal class PaymentAlreadyDoneException : Exception
    {
        public PaymentAlreadyDoneException()
        {
        }

        public PaymentAlreadyDoneException(string? message) : base(message)
        {
        }

        public PaymentAlreadyDoneException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}