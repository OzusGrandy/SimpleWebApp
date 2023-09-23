using SimpleWebApp.Storage.Models;

namespace SimpleWebApp.BusinessLogic.Models
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static EmployeeDto FromEntityModel(DatabaseEmployee employee)
        {
            return new EmployeeDto
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
