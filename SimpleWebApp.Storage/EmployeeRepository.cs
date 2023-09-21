using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SimpleWebApp.CommonModels;
using SimpleWebApp.Storage.EmployeeModels;
using System.Linq;


namespace SimpleWebApp.Storage
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
            using (DatabaseContext db = new DatabaseContext(_options))
            {
                var currentDate = DateTime.Now;
                var employee = new Employee
                {
                    Id = Guid.NewGuid(),
                    FirstName = create.FirstName,
                    LastName = create.LastName,
                    Birthday = create.Birthday,
                    CreatedAt = currentDate,
                    UpdatedAt = currentDate
                };
                db.Employee.Add(ConvertToDatabaseEmployee(employee));
                db.SaveChanges();
                return employee;
            }
        }

        public void Delete(Guid id)
        {
            using (DatabaseContext db = new DatabaseContext(_options))
            {
                db.Employee.Remove(ConvertToDatabaseEmployee(Get(id)));
                db.SaveChanges();
            }
        }

        public Employee? Get(Guid id)
        {
            using (DatabaseContext db = new DatabaseContext(_options))
            {
                return ConvertToEmployee(db.Employee.Find(id.ToString()));
            }
        }

        public Employee Update(EmployeeUpdate model)
        {
            var existingEmployee = Get(model.Id);

            using (DatabaseContext db = new DatabaseContext(_options))
            {
                var currentDate = DateTime.Now;

                existingEmployee.FirstName = model.FirstName;
                existingEmployee.LastName = model.LastName;
                existingEmployee.Birthday = model.Birthday;
                existingEmployee.UpdatedAt = currentDate;

                db.Employee.Update(ConvertToDatabaseEmployee(existingEmployee));
                db.SaveChanges();

                return existingEmployee;
            }
        }

        public PagingResult<Employee> GetPage(EmployeePage getPage)
        {
            var limit = getPage.PageConunt;
            var offset = getPage.Page * limit;
            var sortDirectionTypeString = getPage.SortDirection == SortDirectionType.Asc ? "asc" : "desc";
            var sortingType = GetSortingType(getPage.SortBy);
            var resultList = new List<Employee>();

            using (DatabaseContext db = new DatabaseContext(_options))
            {
                var list = db.Employee.AsQueryable().OrderBy(x => x.CreatedAt).Skip(offset).Take(limit).ToList();

                foreach (var employee in list)
                {
                    resultList.Add(ConvertToEmployee(employee));
                }

                var totalCount = db.Employee.Count();

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

        private DatabaseEmployee ConvertToDatabaseEmployee(Employee employee)
        {
            return new DatabaseEmployee()
            {
                Id = employee.Id.ToString(),
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Birthday = ConvertToUnixTime(employee.Birthday),
                CreatedAt = ConvertToUnixTime(employee.CreatedAt),
                UpdatedAt = ConvertToUnixTime(employee.UpdatedAt)
            };
        }

        private Employee ConvertToEmployee(DatabaseEmployee employee)
        {
            return new Employee()
            {
                Id = Guid.Parse(employee.Id),
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Birthday = ConvertToDateTime(employee.Birthday),
                CreatedAt = ConvertToDateTime(employee.CreatedAt),
                UpdatedAt = ConvertToDateTime(employee.UpdatedAt)
            };
        }
    }
}
