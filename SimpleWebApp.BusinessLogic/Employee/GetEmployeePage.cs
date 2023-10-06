using SimpleWebApp.CommonModels;

namespace SimpleWebApp.BusinessLogic.Employee
{
    public class GetEmployeePage
    {
        public int Page { get; set; }
        public int PageConunt { get; set; }
        public SortDirectionType SortDirection { get; set; }
        public SortBy SortBy { get; set; }
    }
}
