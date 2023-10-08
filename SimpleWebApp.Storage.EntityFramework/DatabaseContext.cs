using Microsoft.EntityFrameworkCore;
using SimpleWebApp.Storage.Models.Employees;
using SimpleWebApp.Storage.Models.Projects;

namespace SimpleWebApp.Storage.EntityFramework
{
    public class DatabaseContext : DbContext
    {
        private readonly DatabaseConnectionOptions _connectionOptions;

        public DbSet<DatabaseEmployee> Employee => Set<DatabaseEmployee>();
        public DbSet<DatabaseProject> Project => Set<DatabaseProject>();

        public DatabaseContext(DatabaseConnectionOptions connectionOptions)
        {
            _connectionOptions = connectionOptions;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionOptions.ConnectionString);
        }
    }
}
