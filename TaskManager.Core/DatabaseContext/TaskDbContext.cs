using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Model;

namespace TaskManager.Core.DatabaseContext
{
    public class TaskDbContext : DbContext
    {
        public virtual DbSet<ExchangeRate> ExchangeRate { get; set; }
        public virtual DbSet<Priority> Priorities { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<WorkTask> WorkTasks { get; set; }

        public TaskDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ExchangeRate>()
                   .Property(b => b.Date)
                   .HasDefaultValueSql("getdate()");

            builder.Entity<ExchangeRate>()
                   .Property(b => b.Id)
                   .UseSqlServerIdentityColumn();

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