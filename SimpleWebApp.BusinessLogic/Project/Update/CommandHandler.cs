﻿using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleWebApp.CommonModels;
using SimpleWebApp.Middleware.CustomExceptions;
using SimpleWebApp.Storage.EntityFramework;

namespace SimpleWebApp.BusinessLogic.Project.Update
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
            new ProjectChange.Validator().ValidateAndThrow(command.Changes);

            var project = await _dbContext.Project
                .Where(x => x.Id == command.Id.ToString())
                .SingleOrDefaultAsync(cancellationToken);

            if (project == null)
            {
                throw new NoFoundException();
            }

            var currentDate = DateTime.Now;

            project.Name = command.Changes.Name;
            project.Description = command.Changes.Description;
            project.UpdatedAt = CommonMethods.ConvertToUnixTime(currentDate);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Project.FromEntityModel(project);
        }
    }
}
