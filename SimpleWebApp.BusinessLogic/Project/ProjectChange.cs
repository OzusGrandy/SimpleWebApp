using FluentValidation;

namespace SimpleWebApp.BusinessLogic.Project
{
    public class ProjectChange
    {
        public string Name { get; set; }
        public string Description { get; set; }

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
