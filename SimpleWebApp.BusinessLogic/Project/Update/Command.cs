using MediatR;

namespace SimpleWebApp.BusinessLogic.Project.Update
{
    public class Command : IRequest<Project>
    {
        public ProjectUpdate ProjectUpdate { get; set; }
    }
}
