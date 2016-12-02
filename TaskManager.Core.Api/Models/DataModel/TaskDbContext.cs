using Microsoft.EntityFrameworkCore;

namespace TaskManager.Core.Api.Models.DataModel
{
    public class TaskDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<Priority> Priorities { get; set; }

        public DbSet<WorkTask> WorkTasks { get; set; }

        public TaskDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}