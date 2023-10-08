using MediatR;

namespace SimpleWebApp.BusinessLogic.Project.Get
{
    public class Query : IRequest<Project>
    {
        public Guid Id { get; set; }
    }
}
