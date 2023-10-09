using MediatR;

namespace SimpleWebApp.BusinessLogic.Project.Update
{
    public record Command(Guid Id, ProjectChange Changes) : IRequest<Project>;
}
