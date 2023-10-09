namespace SimpleWebApp.Storage.EntityFramework.Models
{
    public class DatabaseEmployee
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long Birthday { get; set; }
        public List<DatabaseProject> Projects { get; set; }
        public long CreatedAt { get; set; }
        public long UpdatedAt { get; set; }
    }
}
