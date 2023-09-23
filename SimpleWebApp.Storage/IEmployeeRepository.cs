using SimpleWebApp.CommonModels;
using SimpleWebApp.Storage.Models;

namespace SimpleWebApp.Storage
{
    public interface IEmployeeRepository
    {
        Employee Add(EmployeeCreate create);
        void Delete(Guid id);
        Employee? Get(Guid id);
        Employee Update(EmployeeUpdate model);
        PagingResult<Employee> GetPage(EmployeePage getPage);
    }
}
