using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TaskManager.Api.Models.DataModel
{
    public class TaskDBInitializer : CreateDatabaseIfNotExists<TaskDbContext>
    {
        protected override void Seed(TaskDbContext context)
        {
            context.Priorities.Add(new Priority { Id = 0, Name = "Hight" });
            context.Priorities.Add(new Priority { Id = 1, Name = "Normal" });
            context.Priorities.Add(new Priority { Id = 2, Name = "Low" });

            context.States.Add(new State { Id = 0, Name = "Created" });
            context.States.Add(new State { Id = 1, Name = "Active" });
            context.States.Add(new State { Id = 2, Name = "Closed" });

            context.Users.Add(new User { Id = 1, Name = "test user", Password = "12345" });

            context.Tasks.Add(new WorkTask { Id = 1, CreateDateTime = DateTime.Now, Name = "Temp task", UserId = 1 });
        }
    }
}