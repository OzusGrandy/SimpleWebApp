using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleWebApp.CommonModels;
using SimpleWebApp.Storage.EntityFramework;

namespace SimpleWebApp.BusinessLogic.Employee.GetPage
{
    public class QueryHandler : IRequestHandler<Query, PagingResult<Employee>>
    {
        private readonly DatabaseContext _dbContext;

        public QueryHandler(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagingResult<Employee>> Handle(Query query, CancellationToken cancellationToken)
        {
            var limit = query.EmployeePage.PageConunt;
            var offset = query.EmployeePage.Page * limit;
            var sortDirectionTypeString = query.EmployeePage.SortDirection == SortDirectionType.Asc ? "asc" : "desc";
            var sortingType = CommonMethods.GetSortingType(query.EmployeePage.SortBy);

            var list = await _dbContext.Employee
                .Include(x => x.Projects)
                .AsQueryable()
                .OrderBy(x => x.CreatedAt)
                .Skip(offset).Take(limit)
                .ToListAsync(cancellationToken);

            var totalCount = await _dbContext.Employee.CountAsync(cancellationToken);

            return new PagingResult<Employee>(list.Select(x => Employee.FromEntityModel(x, x.Projects)).ToArray(), totalCount);
        }
    }
}
