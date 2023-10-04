using MediatR;
using SimpleWebApp.CommonModels;

namespace SimpleWebApp.BusinessLogic.Employee.GetPage
{
    public class Query : IRequest<PagingResult<Employee>>
    {
        public GetEmployeePage EmployeePage { get; set; }
    }
}
