using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Api.Models.DataModel
{
    public class TaskDbContext : IdentityDbContext<User>
    {
        public TaskDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}