using MediatR;

namespace SimpleWebApp.BusinessLogic.Employee.Create
{
    public record Command : EmployeeChange, IRequest<Employee>
    {
        public Command(
        string firstName,
        string lastName,
        DateTime birthday,
        ICollection<Guid> projectIds) 
            : base(
                firstName,
                lastName,
                birthday,
                projectIds)
        { }
    }
}
