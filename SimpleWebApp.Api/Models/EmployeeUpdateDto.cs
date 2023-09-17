namespace SimpleWebApp.Api.Models
{
    public class EmployeeUpdateDto : EmployeeChangeDto
    {
        public Guid Id { get; set; }
    }
}
