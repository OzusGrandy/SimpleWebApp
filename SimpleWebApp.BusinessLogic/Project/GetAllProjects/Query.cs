using MediatR;

namespace SimpleWebApp.BusinessLogic.Project.GetAllProjects
{
    public class Query : IRequest<List<Project>>
    {
        public ProjectList ListOptions { get; set; }
    }
}
