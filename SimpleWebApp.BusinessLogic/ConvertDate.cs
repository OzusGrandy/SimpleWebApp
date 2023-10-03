using SimpleWebApp.Storage.Models;

namespace SimpleWebApp.BusinessLogic
{
    public class ConvertDate
    {
        public static long ConvertToUnixTime(DateTime date)
        {
            return ((DateTimeOffset)date).ToUnixTimeSeconds();
        }

        public static string GetSortingType(SortBy sortBy)
        {
            return sortBy switch
            {
                SortBy.updatedAt => "updatedAt",
                SortBy.createdAt => "createdAt",
                _ => "createdAt"
            };
        }
    }
}
