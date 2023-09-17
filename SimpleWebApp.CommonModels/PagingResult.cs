namespace SimpleWebApp.CommonModels
{
    public record PagingResult<T>(IEnumerable<T> Items, long TotalCount);
}
