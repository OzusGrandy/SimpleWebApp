using MediatR;

namespace SimpleWebApp.BusinessLogic.Employee.Delete
{
    public class Command : IRequest<Employee>
    {
        public Guid Id { get; set; }
    }
}
