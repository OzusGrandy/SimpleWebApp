namespace SimpleWebApp.Api.Middleware
{
    public class ValidationFailure
    {
        public static ValidationFailure Create(string propertyName, string errorMessage) => new ValidationFailure(propertyName, errorMessage);

        public ValidationFailure(string propertyName, string errorMessage)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            ErrorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
        }

        public string PropertyName { get; }

        public string ErrorMessage { get; }
    }
}
