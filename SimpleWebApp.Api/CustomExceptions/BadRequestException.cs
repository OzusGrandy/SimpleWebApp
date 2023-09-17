namespace SimpleWebApp.Api.CustomExceptions
{
    public class BadRequestExcepion : Exception
    {
        public BadRequestExcepion() { }
        public BadRequestExcepion(string message) : base(message) { }
    }
}
