using SimpleWebApp.Api.Models;
using SimpleWebApp.Storage;
using SimpleWebApp.BusinessLogic;
using SimpleWebApp.Middleware;
using Microsoft.Extensions.Options;
using SimpleWebApp.Storage.EntityFramework;
using SimpleWebApp.Storage.RawSql;
using MediatR;

namespace SimpleWebApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMediatR(typeof(AssemblyMarker));
            builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddConsole().SetMinimumLevel(LogLevel.Debug));
            builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<DatabaseContext>(sp => 
            {
                var options = sp.GetRequiredService<IOptions<DatabaseConnectionOptions>>();
                return new DatabaseContext(options.Value);
            });
            builder.Services.Configure<AppConfigModel>(builder.Configuration.GetSection(nameof(AppConfigModel)));
            builder.Services.Configure<DatabaseConnectionOptions>(builder.Configuration.GetSection("DatabaseConnection"));
            builder.Services.Configure<ValidationOptions>(builder.Configuration.GetSection("ValidationOptions"));


            var app = builder.Build();

            app.MapControllers();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseMiddleware<CustomExceptionMiddleware>();

            app.Run();
        }
    }
}