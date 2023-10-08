using MediatR;

namespace SimpleWebApp.BusinessLogic.Project.Delete
{
    public class Command : IRequest<Project>
    {
        public Guid Id { get; set; }
    }
}
