using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using SimpleWebApp.CommonModels;
using SimpleWebApp.Storage.RawSql.Models.Projects;

namespace SimpleWebApp.Storage.RawSql
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DatabaseConnectionOptions _options;

        public ProjectRepository(IOptions<DatabaseConnectionOptions> options)
        {
            _options = options.Value;
        }

        public Project Add(ProjectCreate create)
        {
            using (var connection = new SqliteConnection(_options.ConnectionString))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand
                    ("INSERT INTO project (id, name, description, createdAt, updatedAt) " +
                    "VALUES ($id, $name, $description, $createdAt, $updatedAt)", connection);

                var id = Guid.NewGuid();
                var currentDate = DateTime.Now;

                command.Parameters.AddWithValue("$id", id.ToString());
                command.Parameters.AddWithValue("$name", create.Name);
                command.Parameters.AddWithValue("description", create.Description);
                command.Parameters.AddWithValue("$createdAt", CommonMethods.ConvertToUnixTime(currentDate));
                command.Parameters.AddWithValue("$updatedAt", CommonMethods.ConvertToUnixTime(currentDate));
                command.ExecuteNonQuery();

                return new Project
                {
                    Id = id,
                    Name = create.Name,
                    Description = create.Description,
                    CreatedAt = currentDate,
                    UpdatedAt = currentDate
                };
            }
        }

        public void Delete(Guid id)
        {
            using (var connection = new SqliteConnection(_options.ConnectionString))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand($"DELETE FROM project WHERE id=$id", connection);

                command.Parameters.AddWithValue("$id", id.ToString());

                command.ExecuteNonQuery();
            }
        }

        public Project? Get(Guid id)
        {
            using (var connection = new SqliteConnection(_options.ConnectionString))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand("SELECT * FROM project WHERE id=$id", connection);

                command.Parameters.AddWithValue("$id", id.ToString());

                SqliteDataReader reader = command.ExecuteReader();

                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();

                var employee = new Project
                {
                    Id = id,
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    CreatedAt = CommonMethods.ConvertToDateTime(reader.GetInt32(4)),
                    UpdatedAt = CommonMethods.ConvertToDateTime(reader.GetInt32(5))
                };

                reader.Close();

                return employee;
            }

        }

        public Project Update(ProjectUpdate update)
        {
            var existingProject = Get(update.Id);

            using (var connection = new SqliteConnection(_options.ConnectionString))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(
                    "UPDATE project " +
                    "SET " +
                    "name = $name, " +
                    "description = description, " +
                    "updatedAt = $updatedAt " +
                    "WHERE id = $id", connection);

                var currentDate = DateTime.Now;
                command.Parameters.AddWithValue("$id", update.Id.ToString());
                command.Parameters.AddWithValue("name", update.Name);
                command.Parameters.AddWithValue("description", update.Description);
                command.Parameters.AddWithValue("$updatedAt", CommonMethods.ConvertToUnixTime(currentDate));
                command.ExecuteNonQuery();

                var project = new Project
                {
                    Id = update.Id,
                    Name = update.Name,
                    Description = update.Description,
                    CreatedAt = existingProject.CreatedAt,
                    UpdatedAt = currentDate
                };

                return project;
            }

        }

        public List<Project> GetAllProjects(ProjectList listOptions)
        {
            var sortDirectionTypeString = listOptions.SortDirection == SortDirectionType.Asc ? "asc" : "desc";
            var sortingType = CommonMethods.GetSortingType(listOptions.SortBy);
            var resultList = new List<Project>();

            using (var connection = new SqliteConnection(_options.ConnectionString))
            {
                connection.Open();

                SqliteCommand commandPaging = new SqliteCommand($"SELECT * FROM employee ORDER BY {sortingType} {sortDirectionTypeString}", connection);

                SqliteDataReader reader = commandPaging.ExecuteReader();

                while (reader.Read())
                {
                    var project = new Project
                    {
                        Id = Guid.Parse(reader.GetString(0)),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        CreatedAt = CommonMethods.ConvertToDateTime(reader.GetInt32(3)),
                        UpdatedAt = CommonMethods.ConvertToDateTime(reader.GetInt32(4))
                    };

                    resultList.Add(project);
                }

                return resultList;
            }
        }
    }
}
