namespace SimpleWebApp.Storage.Models.Employees
{
    public class DatabaseEmployee
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long Birthday { get; set; }
        public long CreatedAt { get; set; }
        public long UpdatedAt { get; set; }
    }
}
