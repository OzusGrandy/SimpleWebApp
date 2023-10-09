using SimpleWebApp.Storage.RawSql.Models.Projects;

namespace SimpleWebApp.Storage.RawSql.Models.Employees
{
    public class EmployeeUpdate
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public List<Project> AvailableProjects { get; set; }
    }
}
