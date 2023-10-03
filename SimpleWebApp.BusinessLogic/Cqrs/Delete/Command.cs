using MediatR;
using SimpleWebApp.BusinessLogic.Models;

namespace SimpleWebApp.BusinessLogic.Cqrs.Delete
{
    public class Command : IRequest<EmployeeDto>
    {
        public Guid Id { get; set; }
    }
}
