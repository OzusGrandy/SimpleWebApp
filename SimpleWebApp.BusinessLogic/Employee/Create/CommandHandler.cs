using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using SimpleWebApp.Storage.EntityFramework;
using SimpleWebApp.Storage.Models;

namespace SimpleWebApp.BusinessLogic.Employee.Create
{
    public class CommandHandler : IRequestHandler<Command, Employee>
    {
        private readonly DatabaseContext _dbContext;
        private readonly ValidationOptions _validationOptions;

        public CommandHandler(DatabaseContext dbContext, IOptions<ValidationOptions> validationOptions)
        {
            _dbContext = dbContext;
            _validationOptions = validationOptions.Value;
        }

        public async Task<Employee> Handle(Command command, CancellationToken cancellationToken)
        {
            new EmployeeChange.Validator(_validationOptions).ValidateAndThrow(command.EmployeeCreate);

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

            return Employee.FromEntityModel(employee);
        }
    }
}
