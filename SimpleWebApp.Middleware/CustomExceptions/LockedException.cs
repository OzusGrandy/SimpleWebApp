namespace SimpleWebApp.Middleware.CustomExceptions
{
    public class LockedException : Exception
    {
        public LockedException() { }
        public LockedException(string message) : base(message) { }
    }
}
