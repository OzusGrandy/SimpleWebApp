using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SimpleWebApp.CommonModels;
using SimpleWebApp.Middleware.CustomExceptions;
using SimpleWebApp.Storage.EntityFramework;
using SimpleWebApp.Storage.EntityFramework.Models;

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
            new EmployeeChange.Validator(_validationOptions).ValidateAndThrow(command.Changes);

            var currentDate = DateTime.Now;

            var employee = await _dbContext.Employee
                .Where(x => x.Id == command.Id.ToString())
                .SingleOrDefaultAsync(cancellationToken);

            if (employee == null)
            {
                throw new NoFoundException();
            }

            //var projectsList = Employee.GetDatabaseProjectsList(command.Changes.AvailableProjects);

            employee.FirstName = command.Changes.FirstName;
            employee.LastName = command.Changes.LastName;
            employee.Birthday = CommonMethods.ConvertToUnixTime(command.Changes.Birthday);
            //employee.Projects = projectsList;
            employee.UpdatedAt = CommonMethods.ConvertToUnixTime(currentDate);

            //_dbContext.Project.AttachRange(projectsList.ToArray());

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Employee.FromEntityModel(employee, Array.Empty<DatabaseProject>());
        }
    }
}
