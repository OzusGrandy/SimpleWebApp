using MediatR;

namespace SimpleWebApp.BusinessLogic.Employee.Create
{
    public class Command : IRequest<Employee>
    {
        public EmployeeCreate EmployeeCreate { get; set; }
    }
}
