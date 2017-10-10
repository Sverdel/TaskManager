using Microsoft.AspNet.SignalR;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using TaskManager.Api.Models;
using TaskManager.Api.Models.DataModel;

namespace TaskManager.Api.Controllers
{
    [RoutePrefix("api/tasks/{userId:int}/{token}")]
    public class TaskController : ApiController
    {
        private readonly TaskDbContext _dbContext = new TaskDbContext();
        private readonly IHubContext _hub = GlobalHost.ConnectionManager.GetHubContext<TaskHub>();

        [Route()]
        public async Task<IHttpActionResult> GetList(int userId, string token)
        {
            var test = _dbContext.Tasks.Where(x => x.UserId == userId).ToList().Select(x => new TaskDto { Id = x.Id, Name = x.Name });
            return Ok(test);
        }

        [Route("{id:int}", Name = "GetTaskRoute")]
        public async Task<IHttpActionResult> GetTask(int userId, string token, int id)
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
        public async Task<IHttpActionResult> PostTask(int userId, string token, [FromBody]WorkTask task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_dbContext.Tasks.FirstOrDefault(x => x.UserId == userId && x.Name == task.Name) != null)
            {
                return BadRequest("Task with the same name already exists");
            }

            task.CreateDateTime = DateTime.Now;
            task.ChangeDatetime = DateTime.Now;

            _dbContext.Tasks.Add(task);
            await _dbContext.SaveChangesAsync();

            var newTask = await _dbContext.Tasks.FindAsync(task.Id);
            _hub.Clients.Group(userId.ToString(), TaskHub.ConnectionCache[token]).createTask(newTask);

            return CreatedAtRoute("GetTaskRoute", new { userId, token, task.Id }, newTask);
        }

        [Route("{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> PutTask(int userId, string token, long id, [FromBody]WorkTask task)
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
                _hub.Clients.Group(userId.ToString(), TaskHub.ConnectionCache[token]).editTask(task);
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
        public async Task<IHttpActionResult> DeleteTask(int userId, string token, long id)
        {
            WorkTask workTask = await _dbContext.Tasks.FindAsync(id);
            if (workTask == null)
            {
                return NotFound();
            }

            _dbContext.Tasks.Remove(workTask);
            await _dbContext.SaveChangesAsync();

            _hub.Clients.Group(userId.ToString(), TaskHub.ConnectionCache[token]).deleteTask(workTask);

            return Ok(new TaskDto { Id = workTask.Id, Name = workTask.Name });
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