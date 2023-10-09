namespace SimpleWebApp.Storage.EntityFramework.Models
{
    public class DatabaseProject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<DatabaseEmployee> Employees { get; set; }
        public long CreatedAt { get; set; }
        public long UpdatedAt { get; set; }
    }
}
