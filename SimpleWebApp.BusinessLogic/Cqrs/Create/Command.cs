using MediatR;
using SimpleWebApp.BusinessLogic.Models;

namespace SimpleWebApp.BusinessLogic.Cqrs.Create
{
    public class Command : IRequest<EmployeeDto>
    {
        public EmployeeCreateDto EmployeeCreate { get; set; }
    }
}
