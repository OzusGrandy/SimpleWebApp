using MediatR;
using SimpleWebApp.BusinessLogic.Models;
using SimpleWebApp.CommonModels;

namespace SimpleWebApp.BusinessLogic.Cqrs.GetPage
{
    public class Query : IRequest<PagingResult<EmployeeDto>>
    {
        public GetEmployeePageDto EmployeePage { get; set; }
    }
}
