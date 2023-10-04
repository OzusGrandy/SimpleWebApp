using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SimpleWebApp.Middleware.CustomExceptions;
using SimpleWebApp.Storage.EntityFramework;

namespace SimpleWebApp.BusinessLogic.Employee.Update
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
            new EmployeeChange.Validator(_validationOptions).ValidateAndThrow(command.EmployeeUpdate);

            var currentDate = DateTime.Now;

            var employee = await _dbContext.Employee
                .Where(x => x.Id == command.EmployeeUpdate.Id.ToString())
                .SingleOrDefaultAsync(cancellationToken);

            if (employee == null)
            {
                throw new NoFoundException();
            }

            employee.FirstName = command.EmployeeUpdate.FirstName;
            employee.LastName = command.EmployeeUpdate.LastName;
            employee.Birthday = ConvertDate.ConvertToUnixTime(command.EmployeeUpdate.Birthday);
            employee.UpdatedAt = ConvertDate.ConvertToUnixTime(currentDate);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Employee.FromEntityModel(employee);
        }
    }
}
