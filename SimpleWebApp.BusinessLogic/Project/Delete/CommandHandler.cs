using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleWebApp.Storage.EntityFramework;

namespace SimpleWebApp.BusinessLogic.Project.Delete
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
            var project = await _dbContext.Project
                .Where(x => x.Id == command.Id.ToString())
                .SingleOrDefaultAsync(cancellationToken);

            if (project == null)
            {
                return default;
            }

            _dbContext.Project.Remove(project);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return default;
        }
    }
}
