namespace SimpleWebApp.Api.CustomExceptions
{
    public class NoFoundException : Exception
    {
        public NoFoundException() { }
        public NoFoundException(string message) : base(message) { }
    }
}
