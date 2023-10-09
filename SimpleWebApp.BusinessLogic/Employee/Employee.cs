using SimpleWebApp.CommonModels;
using SimpleWebApp.Storage.EntityFramework.Models;

namespace SimpleWebApp.BusinessLogic.Employee
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public List<IdName> Projects { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static Employee FromEntityModel(DatabaseEmployee employee, IEnumerable<DatabaseProject> projects)
        {
            return new Employee
            {
                Id = Guid.Parse(employee.Id),
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Birthday = CommonMethods.ConvertToDateTime(employee.Birthday),
                Projects = projects.Select(x => new IdName(Guid.Parse(x.Id), x.Name)).ToList(),
                CreatedAt = CommonMethods.ConvertToDateTime(employee.CreatedAt),
                UpdatedAt = CommonMethods.ConvertToDateTime(employee.UpdatedAt)
            };
        }
    }
}
