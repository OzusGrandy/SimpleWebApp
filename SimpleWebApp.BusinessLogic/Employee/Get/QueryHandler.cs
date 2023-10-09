using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleWebApp.Middleware.CustomExceptions;
using SimpleWebApp.Storage.EntityFramework;
using SimpleWebApp.Storage.EntityFramework.Models;

namespace SimpleWebApp.BusinessLogic.Employee.Get
{
    public class QueryHandler : IRequestHandler<Query, Employee>
    {
        private readonly DatabaseContext _dbContext;

        public QueryHandler(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Employee> Handle(Query query, CancellationToken cancellationToken)
        {
            var employee = await _dbContext.Employee
                .Where(x => x.Id == query.Id.ToString())
                .SingleOrDefaultAsync(cancellationToken);

            if (employee == null)
            {
                throw new NoFoundException();
            }

            return Employee.FromEntityModel(employee, Array.Empty<DatabaseProject>());
        }
    }
}
