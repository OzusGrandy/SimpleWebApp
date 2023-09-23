using FluentValidation;
using Microsoft.Extensions.Options;
using SimpleWebApp.BusinessLogic.Models;
using SimpleWebApp.CommonModels;
using SimpleWebApp.Middleware.CustomExceptions;
using SimpleWebApp.Storage.EntityFramework;
using SimpleWebApp.Storage.Models;

namespace SimpleWebApp.BusinessLogic
{
    public class EmployeeManager
    {
        private readonly ValidationOptions _validationOptions;
        private readonly DatabaseContext _databaseContext;

        public EmployeeManager(
            IOptions<ValidationOptions> validationOptions, 
            DatabaseContext dbContext)
        {
            _validationOptions = validationOptions.Value;
            _databaseContext = dbContext;
        }

        public EmployeeDto Add(EmployeeCreateDto createDto)
        {
            new EmployeeChangeDto.Validator(_validationOptions).ValidateAndThrow(createDto);

            var currentDate = DateTime.Now;

            var employee = new DatabaseEmployee
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = createDto.FirstName,
                LastName = createDto.LastName,
                Birthday = ConvertToUnixTime(createDto.Birthday),
                CreatedAt = ConvertToUnixTime(currentDate),
                UpdatedAt = ConvertToUnixTime(currentDate)
            };

            _databaseContext.Employee.Add(employee);
            _databaseContext.SaveChanges();

            return EmployeeDto.FromEntityModel(employee);
        }

        public void Delete(Guid id)
        {
            var employee = _databaseContext.Employee
                .Where(x => x.Id == id.ToString())
                .SingleOrDefault();

            if (employee == null)
            {
                return;
            }

            _databaseContext.Employee.Remove(employee);
            _databaseContext.SaveChanges();
        }

        public EmployeeDto Get(Guid id)
        {
            var employee = _databaseContext.Employee
                .Where(x => x.Id == id.ToString())
                .SingleOrDefault();

            if (employee == null)
            {
                throw new NoFoundException();
            }

            return EmployeeDto.FromEntityModel(employee);
        }

        public EmployeeDto Update(EmployeeUpdateDto model)
        {
            new EmployeeChangeDto.Validator(_validationOptions).ValidateAndThrow(model);

            var currentDate = DateTime.Now;

            var employee = _databaseContext.Employee
                .Where(x => x.Id == model.Id.ToString())
                .SingleOrDefault();

            if (employee == null)
            {
                throw new NoFoundException();
            }

            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.Birthday = ConvertToUnixTime(model.Birthday);
            employee.UpdatedAt = ConvertToUnixTime(currentDate);

            _databaseContext.SaveChanges();

            return EmployeeDto.FromEntityModel(employee);
        }

        public PagingResult<EmployeeDto> GetPage(GetEmployeePageDto getPage)
        {
            var limit = getPage.PageConunt;
            var offset = getPage.Page * limit;
            var sortDirectionTypeString = getPage.SortDirection == SortDirectionType.Asc ? "asc" : "desc";
            var sortingType = GetSortingType(getPage.SortBy);

            var list = _databaseContext.Employee
                .AsQueryable()
                .OrderBy(x => x.CreatedAt)
                .Skip(offset).Take(limit)
                .ToList();

            var totalCount = _databaseContext.Employee.Count();

            return new PagingResult<EmployeeDto>(list.Select(x => EmployeeDto.FromEntityModel(x)).ToArray(), totalCount);
        }

        private long ConvertToUnixTime(DateTime date)
        {
            return ((DateTimeOffset)date).ToUnixTimeSeconds();
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
