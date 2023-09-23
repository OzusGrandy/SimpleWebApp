using SimpleWebApp.CommonModels;

namespace SimpleWebApp.BusinessLogic.Models
{
    public class GetEmployeePageDto
    {
        public int Page { get; set; }
        public int PageConunt { get; set; }
        public SortDirectionType SortDirection { get; set; }
        public Storage.Models.SortBy SortBy { get; set; }
    }
}
