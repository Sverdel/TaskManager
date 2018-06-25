using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TaskManager.Api.Models.DataModel
{
    public class TaskDbContextFactory : IDesignTimeDbContextFactory<TaskDbContext>
    {
        public TaskDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TaskDbContext>();
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=TaskManager;Integrated Security=True;Connect Timeout=30");

            return new TaskDbContext(optionsBuilder.Options);
        }
    }
}