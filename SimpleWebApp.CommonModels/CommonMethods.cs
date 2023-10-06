namespace SimpleWebApp.CommonModels
{
    public class CommonMethods
    {
        public static long ConvertToUnixTime(DateTime date)
        {
            return ((DateTimeOffset)date).ToUnixTimeSeconds();
        }

        public static DateTime ConvertToDateTime(long unixTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
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
