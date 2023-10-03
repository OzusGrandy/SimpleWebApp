using MediatR;
using SimpleWebApp.BusinessLogic.Models;

namespace SimpleWebApp.BusinessLogic.Cqrs.Get
{
    public class Query : IRequest<EmployeeDto>
    {
        public Guid Id { get; set; }
    }
}
