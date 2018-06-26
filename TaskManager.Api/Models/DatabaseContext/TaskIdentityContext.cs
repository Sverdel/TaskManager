using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Models.DataModel;

namespace TaskManager.Api.Models.DatabaseContext
{
    public class TaskIdentityContext : IdentityDbContext<User>
    {
        public TaskIdentityContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}