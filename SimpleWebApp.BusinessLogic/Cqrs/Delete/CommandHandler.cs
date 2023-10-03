using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleWebApp.BusinessLogic.Models;
using SimpleWebApp.Storage.EntityFramework;

namespace SimpleWebApp.BusinessLogic.Cqrs.Delete
{
    public class CommandHandler : IRequestHandler<Command, EmployeeDto>
    {
        private readonly DatabaseContext _dbContext;
        public CommandHandler(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<EmployeeDto> Handle(Command command, CancellationToken cancellationToken)
        {
            var employee = await _dbContext.Employee
                .Where(x => x.Id == command.Id.ToString())
                .SingleOrDefaultAsync(cancellationToken);

            if (employee == null)
            {
                return default;
            }

            _dbContext.Employee.Remove(employee);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return default;
        }
    }
}
