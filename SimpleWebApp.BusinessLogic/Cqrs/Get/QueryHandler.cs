using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleWebApp.BusinessLogic.Models;
using SimpleWebApp.Middleware.CustomExceptions;
using SimpleWebApp.Storage.EntityFramework;

namespace SimpleWebApp.BusinessLogic.Cqrs.Get
{
    public class QueryHandler : IRequestHandler<Query, EmployeeDto>
    {
        private readonly DatabaseContext _dbContext;

        public QueryHandler(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<EmployeeDto> Handle(Query query, CancellationToken cancellationToken)
        {
            var employee = await _dbContext.Employee
                .Where(x => x.Id == query.Id.ToString())
                .SingleOrDefaultAsync(cancellationToken);

            if (employee == null)
            {
                throw new NoFoundException();
            }

            return EmployeeDto.FromEntityModel(employee);
        }
    }
}
