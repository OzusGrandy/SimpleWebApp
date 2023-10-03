using MediatR;
using SimpleWebApp.BusinessLogic.Models;

namespace SimpleWebApp.BusinessLogic.Cqrs.Update
{
    public class Command : IRequest<EmployeeDto>
    {
        public EmployeeUpdateDto EmployeeUpdate { get; set; }
    }
}
