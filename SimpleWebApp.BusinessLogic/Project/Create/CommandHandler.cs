using FluentValidation;
using MediatR;
using SimpleWebApp.CommonModels;
using SimpleWebApp.Storage.EntityFramework;
using SimpleWebApp.Storage.Models.Projects;

namespace SimpleWebApp.BusinessLogic.Project.Create
{
    public class CommandHandler : IRequestHandler<Command, Project>
    {
        private readonly DatabaseContext _dbContext;

        public CommandHandler(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Project> Handle(Command command, CancellationToken cancellationToken)
        {
            new ProjectChange.Validator().ValidateAndThrow(command.ProjectCreate);

            var currentDate = DateTime.Now;

            var project = new DatabaseProject
            {
                Id = Guid.NewGuid().ToString(),
                Name = command.ProjectCreate.Name,
                Description = command.ProjectCreate.Description,
                CreatedAt = CommonMethods.ConvertToUnixTime(currentDate),
                UpdatedAt = CommonMethods.ConvertToUnixTime(currentDate)
            };

            await _dbContext.Project.AddAsync(project, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Project.FromEntityModel(project);
        }
    }
}
