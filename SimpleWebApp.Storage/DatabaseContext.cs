using Microsoft.EntityFrameworkCore;
using SimpleWebApp.Storage.EmployeeModels;

namespace SimpleWebApp.Storage
{
    public class DatabaseContext : DbContext
    {
        private readonly DatabaseConnectionOptions _connectionOptions;

        public DbSet<DatabaseEmployee> Employee => Set<DatabaseEmployee>();

        public DatabaseContext(DatabaseConnectionOptions connectionOptions)
        {
            _connectionOptions = connectionOptions;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionOptions.ConnectionString);
        }
    }
}
