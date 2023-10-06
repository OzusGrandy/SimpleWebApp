using SimpleWebApp.Storage.Models.Projects;

namespace SimpleWebApp.Storage.RawSql
{
    public interface IProjectRepository
    {
        Project Add(ProjectCreate create);
        void Delete(Guid id);
        Project? Get(Guid id);
        List<Project> GetAllProjects(ProjectList listOptions);
        Project Update(ProjectUpdate update);
    }
}