using MediatR;

namespace SimpleWebApp.BusinessLogic.Employee.Update
{
    public record Command(Guid Id, EmployeeChange Changes) : IRequest<Employee>;
}
