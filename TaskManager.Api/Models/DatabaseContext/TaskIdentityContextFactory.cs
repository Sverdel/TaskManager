using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TaskManager.Api.Models.DatabaseContext
{
    public class TaskIdentityContextFactory : IDesignTimeDbContextFactory<TaskIdentityContext>
    {
        public TaskIdentityContext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(AppContext.BaseDirectory)
               .AddJsonFile("appsettings.json")
               .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
               .AddEnvironmentVariables();

            var config = builder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<TaskIdentityContext>();
            optionsBuilder.UseSqlServer(config["Data:DefaultConnection:ConnectionString"]);

            return new TaskIdentityContext(optionsBuilder.Options);
        }
    }
}