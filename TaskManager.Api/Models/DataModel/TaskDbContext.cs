using System.Data.Entity;

namespace TaskManager.Api.Models.DataModel
{
    public class TaskDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<Priority> Priorities { get; set; }

        public DbSet<WorkTask> Tasks { get; set; }

        public TaskDbContext() : base("TaskDBConnectionString")
        {
        }
    }
}