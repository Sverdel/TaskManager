using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Model;

namespace TaskManager.Api.Models.DataModel
{
    public class TaskDbContext : IdentityDbContext<User>
    {
        public virtual DbSet<ExchangeRate> ExchangeRate { get; set; }
        public virtual DbSet<Priority> Priorities { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<WorkTask> WorkTasks { get; set; }

        public TaskDbContext(DbContextOptions options) : base(options)
        {
        }

        public TaskDbContext CreateDbContext(string[] args) => throw new System.NotImplementedException();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Priority>().HasData(
                new Priority { Id = 1, Name = "Hight" },
                new Priority { Id = 2, Name = "Normal" },
                new Priority { Id = 3, Name = "Low" });

            builder.Entity<State>().HasData(
                new State { Id = 1, Name = "Created" },
                new State { Id = 2, Name = "Active" },
                new State { Id = 3, Name = "Closed" });
        }
    }
}