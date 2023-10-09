using SimpleWebApp.Storage.RawSql.Models.Projects;

namespace SimpleWebApp.Storage.RawSql.Models.Employees
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public List<Project> AvailableProjects { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
