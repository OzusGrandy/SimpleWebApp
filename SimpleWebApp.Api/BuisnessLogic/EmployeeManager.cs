using Microsoft.Extensions.Options;
using SimpleWebApp.Api.CustomExceptions;
using SimpleWebApp.Api.Models;
using SimpleWebApp.CommonModels;
using SimpleWebApp.Storage.EmployeeModels;
using SimpleWebApp.Storage;
using FluentValidation;

namespace SimpleWebApp.Api.BuisnessLogic
{
    public class EmployeeManager
    {
        private readonly ValidationOptions _validationOptions;
        private IEmployeeRepository _storage;

        public EmployeeManager(IEmployeeRepository storage, IOptions<ValidationOptions> validationOptions)
        {
            _storage = storage;
            _validationOptions = validationOptions.Value;
        }

        public EmployeeDto Add(EmployeeCreateDto createDto)
        {
            new EmployeeChangeDto.Validator(_validationOptions).ValidateAndThrow(createDto);

            var employee = new EmployeeCreate
            {
                Birthday = createDto.Birthday,
                FirstName = createDto.FirstName,
                LastName = createDto.LastName
            };

            var result = _storage.Add(employee);

            return new EmployeeDto
            {
                Id = result.Id,
                FirstName = result.FirstName,
                LastName = result.LastName,
                Birthday = result.Birthday,
                CreatedAt = result.CreatedAt,
                UpdatedAt = result.UpdatedAt
            };
        }

        public void Delete(Guid id)
        {
            _storage.Delete(id);
        }

        public EmployeeDto Get(Guid id)
        {
            var employee = _storage.Get(id);
            if (employee == null)
            {
                throw new NoFoundException();
            }
            var result = new EmployeeDto
            {
                Id = id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Birthday = employee.Birthday,
                CreatedAt = employee.CreatedAt,
                UpdatedAt = employee.UpdatedAt
            };
            return result;
        }

        public EmployeeDto Update(EmployeeUpdateDto model)
        {
            new EmployeeChangeDto.Validator(_validationOptions).ValidateAndThrow(model);

            var employee = new EmployeeUpdate
            {
                Id = model.Id,
                Birthday = model.Birthday,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = _storage.Update(employee);

            return new EmployeeDto
            {
                Id = result.Id,
                FirstName = result.FirstName,
                LastName = result.LastName,
                Birthday = result.Birthday,
                CreatedAt = result.CreatedAt,
                UpdatedAt = result.UpdatedAt
            };
        }

        public PagingResult<EmployeeDto> GetPage(GetEmployeePageDto getPage)
        {
            var employeePage = new EmployeePage
            {
                Page = getPage.Page,
                PageConunt = getPage.PageConunt,
                SortBy = getPage.SortBy,
                SortDirection = getPage.SortDirection
            };

            var result = _storage.GetPage(employeePage);

            return new PagingResult<EmployeeDto>(result.Items.Select(x => new EmployeeDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Birthday = x.Birthday,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            }).ToArray(),
            result.TotalCount);
        }
    }
}
