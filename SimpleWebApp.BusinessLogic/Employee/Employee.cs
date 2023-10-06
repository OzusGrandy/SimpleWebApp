using SimpleWebApp.CommonModels;
using SimpleWebApp.Storage.Models.Employees;

namespace SimpleWebApp.BusinessLogic.Employee
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static Employee FromEntityModel(DatabaseEmployee employee)
        {
            return new Employee
            {
                Id = Guid.Parse(employee.Id),
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Birthday = CommonMethods.ConvertToDateTime(employee.Birthday),
                CreatedAt = CommonMethods.ConvertToDateTime(employee.CreatedAt),
                UpdatedAt = CommonMethods.ConvertToDateTime(employee.UpdatedAt)
            };
        }
    }
}
