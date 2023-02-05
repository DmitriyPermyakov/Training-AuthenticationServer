namespace AuthenticationServer.Exceptions
{
    public class RefreshTokenException : Exception
    {
        public RefreshTokenException() { }
        public RefreshTokenException(string message) : base(message) { }
        public RefreshTokenException(string message, Exception innerException) : base(message, innerException) { }
    }
}
