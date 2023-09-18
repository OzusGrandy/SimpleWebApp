namespace SimpleWebApp.Middleware.CustomExceptions
{
    public class ValidationFailureException : Exception
    {
        public ValidationFailureException()
        {
            Errors = new ValidationFailure[0];
        }

        public ValidationFailureException(string message)
            : base(message)
        {
            Errors = new ValidationFailure[] { new ValidationFailure(string.Empty, message) };
        }

        public ValidationFailureException(ValidationFailure error)
        {
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }

            Errors = new ValidationFailure[] { error };
        }

        public ValidationFailureException(IEnumerable<ValidationFailure> errors)
        {
            Errors = errors ?? throw new ArgumentNullException(nameof(errors));
        }

        public IEnumerable<ValidationFailure> Errors { get; }
    }
}
