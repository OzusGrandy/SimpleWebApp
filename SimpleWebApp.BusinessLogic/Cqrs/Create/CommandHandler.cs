using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using SimpleWebApp.BusinessLogic.Models;
using SimpleWebApp.Storage.EntityFramework;
using SimpleWebApp.Storage.Models;

namespace SimpleWebApp.BusinessLogic.Cqrs.Create
{
    public class CommandHandler : IRequestHandler<Command, EmployeeDto>
    {
        private readonly DatabaseContext _dbContext;
        private readonly ValidationOptions _validationOptions;

        public CommandHandler(DatabaseContext dbContext, IOptions<ValidationOptions> validationOptions)
        {
            _dbContext = dbContext;
            _validationOptions = validationOptions.Value;
        }

        public async Task<EmployeeDto> Handle(Command command, CancellationToken cancellationToken)
        {
            new EmployeeChangeDto.Validator(_validationOptions).ValidateAndThrow(command.EmployeeCreate);

            var currentDate = DateTime.Now;

            var employee = new DatabaseEmployee
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = command.EmployeeCreate.FirstName,
                LastName = command.EmployeeCreate.LastName,
                Birthday = ConvertDate.ConvertToUnixTime(command.EmployeeCreate.Birthday),
                CreatedAt = ConvertDate.ConvertToUnixTime(currentDate),
                UpdatedAt = ConvertDate.ConvertToUnixTime(currentDate)
            };

            await _dbContext.Employee.AddAsync(employee, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return EmployeeDto.FromEntityModel(employee);
        }
    }
}
