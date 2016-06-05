using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TaskManager.Api.Models;
using TaskManager.Api.Models.DataModel;

namespace TaskManager.Api.Controllers
{
    [RoutePrefix("api/tasks/{userId:int}")]
    public class TaskController : ApiController
    {
        private TaskDbContext _dbContext = new TaskDbContext();
        private IHubContext _hub = GlobalHost.ConnectionManager.GetHubContext<TaskHub>();

        [Route()]
        public async Task<IHttpActionResult> GetList(int userId)
        {
            return Ok(_dbContext.Tasks.Where(x => x.UserId == userId).ToList().Select(x => new { Id = x.Id, Name = x.Name }));
        }

        [Route("{id:int}", Name = "GetTaskRoute")]
        public async Task<IHttpActionResult> Get(int userId, int id)
        {
            WorkTask workTask = await _dbContext.Tasks.FindAsync(id);
            if (workTask == null)
            {
                return NotFound();
            }

            return Ok(workTask);
        }

        [Route()]
        [HttpPost]
        public async Task<IHttpActionResult> Post(int userId, [FromBody]WorkTask task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            task.CreateDateTime = DateTime.Now;
            task.ChangeDatetime = DateTime.Now;
            _dbContext.Tasks.Add(task);
            await _dbContext.SaveChangesAsync();

            var newTask = await _dbContext.Tasks.FindAsync(task.Id);
            _hub.Clients.Group(userId.ToString()).createTask(newTask);

            return CreatedAtRoute("GetTaskRoute", new { userId, task.Id }, newTask);
        }

        [Route("{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> Put(int userId, int id, [FromBody]WorkTask task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != task.Id)
            {
                return BadRequest();
            }

            task.ChangeDatetime = DateTime.Now;
            _dbContext.Entry(task).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
                _hub.Clients.Group(userId.ToString()).editTask(task);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_dbContext.Tasks.Count(e => e.Id == id) == 0)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }


            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("{id:int}")]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int userId, int id)
        {
            WorkTask workTask = await _dbContext.Tasks.FindAsync(id);
            if (workTask == null)
            {
                return NotFound();
            }

            _dbContext.Tasks.Remove(workTask);
            await _dbContext.SaveChangesAsync();

            _hub.Clients.Group(userId.ToString()).deleteTask(workTask);

            return Ok(new { Id = workTask.Id, Name = workTask.Name });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}