namespace AuthenticationServer.Exceptions
{
    public class RegisterException : Exception
    {
        public RegisterException() { }
        public RegisterException(string message) : base(message) { }
        public RegisterException(string message, Exception innerException) : base(message, innerException) { }
    }
}
