using MediatR;

namespace SimpleWebApp.BusinessLogic.Project.Create
{
    public record Command : ProjectChange, IRequest<Project>
    {
        public Command(string name, string description)
                : base(name, description) { }
    }
}
