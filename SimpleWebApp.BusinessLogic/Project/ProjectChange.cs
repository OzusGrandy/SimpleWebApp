using FluentValidation;

namespace SimpleWebApp.BusinessLogic.Project
{
    public record ProjectChange(
        string Name,
        string Description)
    {
        public class Validator : AbstractValidator<ProjectChange>
        {
            public Validator()
            {
                RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("Empty")
                .MaximumLength(50)
                .WithMessage("ToLong")
                .MinimumLength(2)
                .WithMessage("ToShort");
            }
        }
    }
}
