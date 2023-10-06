using SimpleWebApp.CommonModels;
using SimpleWebApp.Storage.Models.Employees;

namespace SimpleWebApp.Storage.RawSql
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
