using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SimpleWebApp.CommonModels;
using SimpleWebApp.Storage.EntityFramework;
using SimpleWebApp.Storage.EntityFramework.Models;

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
            new EmployeeChange.Validator(_validationOptions).ValidateAndThrow(command);

            var currentDate = DateTime.Now;

            var projectIdStrings = command.ProjectIds.Select(x => x.ToString()).ToArray();

            var projectsList = await _dbContext.Project
                .Where(h => projectIdStrings.Contains(h.Id))
                .ToListAsync(cancellationToken);

            var employee = new DatabaseEmployee
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = command.FirstName,
                LastName = command.LastName,
                Birthday = CommonMethods.ConvertToUnixTime(command.Birthday),
                Projects = projectsList,
                CreatedAt = CommonMethods.ConvertToUnixTime(currentDate),
                UpdatedAt = CommonMethods.ConvertToUnixTime(currentDate)
            };

            await _dbContext.Employee.AddAsync(employee, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Employee.FromEntityModel(employee, projectsList);
        }
    }
}
