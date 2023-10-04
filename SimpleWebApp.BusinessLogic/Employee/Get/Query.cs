using MediatR;

namespace SimpleWebApp.BusinessLogic.Employee.Get
{
    public class Query : IRequest<Employee>
    {
        public Guid Id { get; set; }
    }
}
