using SimpleWebApp.Storage.Models;

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
                Birthday = ConvertToDateTime(employee.Birthday),
                CreatedAt = ConvertToDateTime(employee.CreatedAt),
                UpdatedAt = ConvertToDateTime(employee.UpdatedAt)
            };
        }

        private static DateTime ConvertToDateTime(long unixTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
        }
    }
}
