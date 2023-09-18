using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Options;
using SimpleWebApp.BusinessLogic.Models;
using SimpleWebApp.CommonModels;
using SimpleWebApp.Middleware.CustomExceptions;
using SimpleWebApp.Storage;
using SimpleWebApp.Storage.EmployeeModels;

namespace SimpleWebApp.BusinessLogic
{
    public class EmployeeManager
    {
        private readonly ValidationOptions _validationOptions;
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _storage;

        public EmployeeManager(IEmployeeRepository storage, IOptions<ValidationOptions> validationOptions, IMapper mapper)
        {
            _storage = storage;
            _validationOptions = validationOptions.Value;
            _mapper = mapper;
        }

        public EmployeeDto Add(EmployeeCreateDto createDto)
        {
            new EmployeeChangeDto.Validator(_validationOptions).ValidateAndThrow(createDto);

            return _mapper.Map<EmployeeDto>(_storage.Add(_mapper.Map<EmployeeCreate>(createDto)));
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
            return _mapper.Map<EmployeeDto>(employee);
        }

        public EmployeeDto Update(EmployeeUpdateDto model)
        {
            new EmployeeChangeDto.Validator(_validationOptions).ValidateAndThrow(model);

            return _mapper.Map<EmployeeDto>(_storage.Update(_mapper.Map<EmployeeUpdate>(model)));
        }

        public PagingResult<EmployeeDto> GetPage(GetEmployeePageDto getPage)
        {
            var result = _storage.GetPage(_mapper.Map<EmployeePage>(getPage));

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
