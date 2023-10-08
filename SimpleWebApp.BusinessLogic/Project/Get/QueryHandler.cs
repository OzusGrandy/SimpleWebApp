using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleWebApp.Middleware.CustomExceptions;
using SimpleWebApp.Storage.EntityFramework;

namespace SimpleWebApp.BusinessLogic.Project.Get
{
    public class QueryHandler : IRequestHandler<Query, Project>
    {
        private readonly DatabaseContext _dbContext;

        public QueryHandler(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Project> Handle(Query query, CancellationToken cancellationToken)
        {
            var project = await _dbContext.Project
                .Where(x => x.Id == query.Id.ToString())
                .SingleOrDefaultAsync(cancellationToken);

            if (project == null)
            {
                throw new NoFoundException();
            }

            return Project.FromEntityModel(project);
        }
    }
}
