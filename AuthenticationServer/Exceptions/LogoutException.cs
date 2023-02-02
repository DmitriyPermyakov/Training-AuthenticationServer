namespace AuthenticationServer.Exceptions
{
    public class LogoutException : Exception
    {
        public LogoutException() { }
        public LogoutException(string message) : base(message) { }
        public LogoutException(string message, Exception innerException) : base(message, innerException) { }
    }
}
