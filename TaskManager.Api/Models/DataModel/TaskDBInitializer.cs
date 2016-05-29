using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TaskManager.Api.Models.DataModel
{
    public class TaskDBInitializer : DropCreateDatabaseAlways<TaskDbContext>
    {
        protected override void Seed(TaskDbContext context)
        {
            context.Priorities.Add(new Priority { Id = 0, Name = "Hight" });
            context.Priorities.Add(new Priority { Id = 1, Name = "Normal" });
            context.Priorities.Add(new Priority { Id = 2, Name = "Low" });

            context.States.Add(new State { Id = 0, Name = "Created" });
            context.States.Add(new State { Id = 0, Name = "Active" });
            context.States.Add(new State { Id = 0, Name = "Closed" });
        }
    }
}