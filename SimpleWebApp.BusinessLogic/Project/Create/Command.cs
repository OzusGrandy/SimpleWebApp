using MediatR;

namespace SimpleWebApp.BusinessLogic.Project.Create
{
    public class Command : IRequest<Project>
    {
        public ProjectCreate ProjectCreate { get; set; }
    }
}
