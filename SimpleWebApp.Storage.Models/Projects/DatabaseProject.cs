namespace SimpleWebApp.Storage.Models.Projects
{
    public class DatabaseProject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long CreatedAt { get; set; }
        public long UpdatedAt { get; set; }
    }
}
