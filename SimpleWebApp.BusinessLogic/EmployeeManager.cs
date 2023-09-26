using FluentValidation;
using Microsoft.EntityFrameworkCore;
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

        public async Task<EmployeeDto> Add(EmployeeCreateDto createDto, CancellationToken cancellationToken)
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

            await _databaseContext.Employee.AddAsync(employee, cancellationToken);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return EmployeeDto.FromEntityModel(employee);
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            var employee = await _databaseContext.Employee
                .Where(x => x.Id == id.ToString())
                .SingleOrDefaultAsync(cancellationToken);

            if (employee == null)
            {
                return;
            }

            _databaseContext.Employee.Remove(employee);
            await _databaseContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<EmployeeDto> Get(Guid id, CancellationToken cancellationToken)
        {
            var employee = await _databaseContext.Employee
                .Where(x => x.Id == id.ToString())
                .SingleOrDefaultAsync(cancellationToken);

            if (employee == null)
            {
                throw new NoFoundException();
            }

            return EmployeeDto.FromEntityModel(employee);
        }

        public async Task<EmployeeDto> Update(EmployeeUpdateDto model, CancellationToken cancellationToken)
        {
            new EmployeeChangeDto.Validator(_validationOptions).ValidateAndThrow(model);

            var currentDate = DateTime.Now;

            var employee = await _databaseContext.Employee
                .Where(x => x.Id == model.Id.ToString())
                .SingleOrDefaultAsync(cancellationToken);

            if (employee == null)
            {
                throw new NoFoundException();
            }

            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.Birthday = ConvertToUnixTime(model.Birthday);
            employee.UpdatedAt = ConvertToUnixTime(currentDate);

            await _databaseContext.SaveChangesAsync(cancellationToken);

            return EmployeeDto.FromEntityModel(employee);
        }

        public async Task<PagingResult<EmployeeDto>> GetPage(GetEmployeePageDto getPage, CancellationToken cancellationToken)
        {
            var limit = getPage.PageConunt;
            var offset = getPage.Page * limit;
            var sortDirectionTypeString = getPage.SortDirection == SortDirectionType.Asc ? "asc" : "desc";
            var sortingType = GetSortingType(getPage.SortBy);

            var list = await _databaseContext.Employee
                .AsQueryable()
                .OrderBy(x => x.CreatedAt)
                .Skip(offset).Take(limit)
                .ToListAsync(cancellationToken);

            var totalCount = await _databaseContext.Employee.CountAsync(cancellationToken);

            return new PagingResult<EmployeeDto>(list.Select(x => EmployeeDto.FromEntityModel(x)).ToArray(), totalCount);
        }

        private long ConvertToUnixTime(DateTime date)
        {
            return ((DateTimeOffset)date).ToUnixTimeSeconds();
        }

        private string GetSortingType(SortBy sortBy)
        {
            return sortBy switch
            {
                SortBy.updatedAt => "updatedAt",
                SortBy.createdAt => "createdAt",
                _ => "createdAt"
            };
        }
    }
}
