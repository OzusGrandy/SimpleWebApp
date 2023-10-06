using Microsoft.EntityFrameworkCore;
using SimpleWebApp.Storage.Models.Employees;

namespace SimpleWebApp.Storage.EntityFramework
{
    public class DatabaseContext : DbContext
    {
        private readonly DatabaseConnectionOptions _connectionOptions;

        public DbSet<DatabaseEmployee> Employee => Set<DatabaseEmployee>();

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
