using SimpleWebApp.CommonModels;

namespace SimpleWebApp.Storage.Models.Employees
{
    public class EmployeePage
    {
        public int Page { get; set; }
        public int PageConunt { get; set; }
        public SortDirectionType SortDirection { get; set; }
        public SortBy SortBy { get; set; }
    }
}
