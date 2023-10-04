using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleWebApp.Storage.EntityFramework;

namespace SimpleWebApp.BusinessLogic.Employee.Delete
{
    public class CommandHandler : IRequestHandler<Command, Employee>
    {
        private readonly DatabaseContext _dbContext;
        public CommandHandler(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Employee> Handle(Command command, CancellationToken cancellationToken)
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
