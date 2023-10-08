using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleWebApp.CommonModels;
using SimpleWebApp.Storage.EntityFramework;

namespace SimpleWebApp.BusinessLogic.Project.GetAllProjects
{
    public class QueryHandler : IRequestHandler<Query, List<Project>>
    {
        private readonly DatabaseContext _dbContext;

        public QueryHandler(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Project>> Handle(Query query, CancellationToken cancellationToken)
        {
            var sortDirectionTypeString = query.ListOptions.SortDirection == SortDirectionType.Asc ? "asc" : "desc";
            var sortingType = CommonMethods.GetSortingType(query.ListOptions.SortBy);

            var list = await _dbContext.Project
                .AsQueryable()
                .OrderBy(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

            return new List<Project>(list.Select(x => Project.FromEntityModel(x)).ToArray());
        }
    }
}
