using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManager.Api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using TaskManager.Api.Models;
using TaskManager.Api.Models.DataModel;

namespace TaskManager.Api.Controllers.Tests
{
    [TestClass()]
    public class TaskControllerTests
    {
        [TestMethod()]
        public async Task GetListTest()
        {
            TaskController controller = new TaskController();
            var result = await controller.GetList(1, string.Empty);

            var tasks = (result as OkNegotiatedContentResult<IEnumerable<TaskDto>>).Content;

            Assert.IsNotNull(tasks);
            Assert.AreEqual(10, tasks.Count());

            result = await controller.GetList(2, string.Empty);

            tasks = (result as OkNegotiatedContentResult<IEnumerable<TaskDto>>).Content;

            Assert.IsNotNull(tasks);
            Assert.AreEqual(0, tasks.Count());

        }

        [TestMethod()]
        public async Task GetTest()
        {
            TaskController controller = new TaskController();
            var result = await controller.GetTask(1, string.Empty, 1);

            var task = (result as OkNegotiatedContentResult<WorkTask>).Content;

            Assert.IsNotNull(task);
        }

        [TestMethod()]
        public async Task PostTest()
        {
            string name = "test";
            try
            {
                TaskHub.ConnectionCache.TryAdd(string.Empty, string.Empty);
                WorkTask task = new WorkTask { Name = name, PriorityId = 1, StateId = 1, UserId = 1 };
                TaskController controller = new TaskController();
                var result = await controller.PostTask(1, string.Empty, task);

                var resultTask = (result as CreatedAtRouteNegotiatedContentResult<WorkTask>).Content;

                Assert.IsNotNull(resultTask);
                Assert.AreEqual(name, resultTask.Name, "name");
                Assert.IsNotNull(resultTask.CreateDateTime, "createDateTime");
                Assert.IsNotNull(resultTask.ChangeDatetime, "ChangeDateTime");
            }
            finally
            {
                TaskDbContext context = new TaskDbContext();
                var task = context.Tasks.FirstOrDefault(x => x.Name == name);
                if (task != null)
                {
                    context.Tasks.Remove(task);
                    context.SaveChanges();
                }
            }
        }

        [TestMethod()]
        public async Task PutTest()
        {
            TaskHub.ConnectionCache.TryAdd(string.Empty, string.Empty);
            TaskController controller = new TaskController();
            var result = await controller.GetTask(1, string.Empty, 1);

            var task = (result as OkNegotiatedContentResult<WorkTask>).Content;

            var description = Guid.NewGuid().ToString();
            task.Description = description;

            await controller.PutTask(1, string.Empty, 1, task);

            TaskDbContext context = new TaskDbContext();
            task = await context.Tasks.FindAsync(1);

            Assert.IsNotNull(task);
            Assert.AreEqual(description, task.Description, "description");
        }

        [TestMethod()]
        public async Task DeleteTest()
        {
            TaskDbContext context = new TaskDbContext();
            string name = "test";
            try
            {
                TaskHub.ConnectionCache.TryAdd(string.Empty, string.Empty);
                WorkTask task = new WorkTask { Name = name, PriorityId = 1, StateId = 1, UserId = 1 };
                TaskController controller = new TaskController();
                var result = await controller.PostTask(1, string.Empty, task);
                var resultTask = (result as CreatedAtRouteNegotiatedContentResult<WorkTask>).Content;

                result = await controller.DeleteTask(1, string.Empty, resultTask.Id);

                var dbTask = context.Tasks.FirstOrDefault(x => x.Name == name);

                Assert.IsNull(dbTask);
            }
            finally
            {
                var task = context.Tasks.FirstOrDefault(x => x.Name == name);
                if (task != null)
                {
                    context.Tasks.Remove(task);
                    context.SaveChanges();
                }
            }
        }
    }
}