using MediatR;

namespace SimpleWebApp.BusinessLogic.Employee.Update
{
    public class Command : IRequest<Employee>
    {
        public EmployeeUpdate EmployeeUpdate { get; set; }
    }
}
