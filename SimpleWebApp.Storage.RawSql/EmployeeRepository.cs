using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using SimpleWebApp.CommonModels;
using SimpleWebApp.Storage.Models;

namespace SimpleWebApp.Storage.RawSql
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DatabaseConnectionOptions _options;
        public EmployeeRepository(IOptions<DatabaseConnectionOptions> options)
        {
            _options = options.Value;
        }

        public Employee Add(EmployeeCreate create)
        {
            using (var connection = new SqliteConnection(_options.ConnectionString))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand
                    ("INSERT INTO employee (id, firstName, lastName, birthday, createdAt, updatedAt) " +
                    "VALUES ($id, $firstName, $lastName, $birthday, $createdAt, $updatedAt)", connection);

                var id = Guid.NewGuid();
                var currentDate = DateTime.Now;
                command.Parameters.AddWithValue("$id", id.ToString());
                command.Parameters.AddWithValue("$firstName", create.FirstName);
                command.Parameters.AddWithValue("$lastName", create.LastName);
                command.Parameters.AddWithValue("$birthday", ConvertToUnixTime(create.Birthday));
                command.Parameters.AddWithValue("$createdAt", ConvertToUnixTime(currentDate));
                command.Parameters.AddWithValue("$updatedAt", ConvertToUnixTime(currentDate));
                command.ExecuteNonQuery();

                return new Employee
                {
                    Id = id,
                    FirstName = create.FirstName,
                    LastName = create.LastName,
                    Birthday = create.Birthday,
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

                SqliteCommand command = new SqliteCommand($"DELETE FROM employee WHERE id=$id", connection);

                command.Parameters.AddWithValue("$id", id.ToString());

                command.ExecuteNonQuery();
            }
        }

        public Employee? Get(Guid id)
        {
            using (var connection = new SqliteConnection(_options.ConnectionString))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand("SELECT * FROM employee WHERE id=$id", connection);

                command.Parameters.AddWithValue("$id", id.ToString());

                SqliteDataReader reader = command.ExecuteReader();

                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();

                var employee = new Employee
                {
                    Id = id,
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Birthday = ConvertToDateTime(reader.GetInt32(3)),
                    CreatedAt = ConvertToDateTime(reader.GetInt32(4)),
                    UpdatedAt = ConvertToDateTime(reader.GetInt32(5))
                };

                reader.Close();

                return employee;
            }
        }

        public Employee Update(EmployeeUpdate model)
        {
            var existingEmployee = Get(model.Id);

            using (var connection = new SqliteConnection(_options.ConnectionString))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(
                    "UPDATE employee " +
                    "SET " +
                    "firstName = $firstName, " +
                    "lastName = $lastName, " +
                    "birthday = $birthday, " +
                    "updatedAt = $updatedAt " +
                    "WHERE id = $id", connection);

                var currentDate = DateTime.Now;
                command.Parameters.AddWithValue("$id", model.Id.ToString());
                command.Parameters.AddWithValue("$firstName", model.FirstName);
                command.Parameters.AddWithValue("$lastName", model.LastName);
                command.Parameters.AddWithValue("$birthday", ConvertToUnixTime(model.Birthday));
                command.Parameters.AddWithValue("$updatedAt", ConvertToUnixTime(currentDate));
                command.ExecuteNonQuery();

                var employee = new Employee
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Birthday = model.Birthday,
                    CreatedAt = existingEmployee.CreatedAt,
                    UpdatedAt = currentDate
                };

                return employee;
            }
        }

        public PagingResult<Employee> GetPage(EmployeePage getPage)
        {
            var limit = getPage.PageConunt;
            var offset = getPage.Page * limit;
            var sortDirectionTypeString = getPage.SortDirection == SortDirectionType.Asc ? "asc" : "desc";
            var sortingType = GetSortingType(getPage.SortBy);
            var resultList = new List<Employee>();

            using (var connection = new SqliteConnection(_options.ConnectionString))
            {
                connection.Open();

                SqliteCommand commandPaging = new SqliteCommand($"SELECT * FROM employee ORDER BY {sortingType} {sortDirectionTypeString} LIMIT {limit} OFFSET {offset}", connection);

                SqliteDataReader reader = commandPaging.ExecuteReader();

                while (reader.Read())
                {
                    var employee = new Employee
                    {
                        Id = Guid.Parse(reader.GetString(0)),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Birthday = ConvertToDateTime(reader.GetInt32(3)),
                        CreatedAt = ConvertToDateTime(reader.GetInt32(4)),
                        UpdatedAt = ConvertToDateTime(reader.GetInt32(5))
                    };

                    resultList.Add(employee);
                }

                SqliteCommand commandTotalCount = new SqliteCommand($"SELECT count(id) FROM employee", connection);

                SqliteDataReader totalCountReader = commandTotalCount.ExecuteReader();

                totalCountReader.Read();

                var totalCount = totalCountReader.GetInt64(0);

                return new PagingResult<Employee>(resultList, totalCount);
            }
        }

        private long ConvertToUnixTime(DateTime date)
        {
            return ((DateTimeOffset)date).ToUnixTimeSeconds();
        }

        private DateTime ConvertToDateTime(long unixTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
        }

        private string GetSortingType(SortBy sortBy)
        {
            switch (sortBy)
            {
                case SortBy.updatedAt: return "updatedAt";
                case SortBy.createdAt: return "createdAt";
                default: return "createdAt";
            }
        }
    }
}
