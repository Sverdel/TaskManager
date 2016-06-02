using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TaskManager.Api.Models.DataModel;
using TaskManager.Api.Models.Dto;

namespace TaskManager.Api.Controllers
{
    [RoutePrefix("api/users/{userId:int}")]
    public class TaskController : ApiController
    {
        private TaskDbContext _dbContext = new TaskDbContext();

        [Route("tasks/all", Name = "GetTaskRoute")]
        public async Task<IHttpActionResult> GetList(int userId)
        {
            return Ok(_dbContext.Tasks.Where(x => x.UserId == userId).ToList().Select(x => new { Id = x.Id, Name = x.Name }));
        }

        [Route("tasks/{id:int}")]
        public async Task<IHttpActionResult> Get(int userId, int id)
        {
            WorkTask workTask = await _dbContext.Tasks.FindAsync(id);
            if (workTask == null)
            {
                return NotFound();
            }

            return Ok(ToDto(workTask));
        }

        [Route("tasks/{id:int}")]
        [HttpPost]
        public async Task<IHttpActionResult> Post(int userId, [FromBody]TaskDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = FromDto(taskDto);
            _dbContext.Tasks.Add(task);
            await _dbContext.SaveChangesAsync();

            return CreatedAtRoute("GetTaskRoute", new { userId, task.Id }, task);
        }

        [Route("tasks/{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> Put(int userId, int id, [FromBody]TaskDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != taskDto.Id)
            {
                return BadRequest();
            }

            var task = FromDto(taskDto);
            _dbContext.Entry(task).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
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

        [Route("tasks/{id:int}")]
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

            return Ok(ToDto(workTask));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }

        private WorkTask FromDto(TaskDto dto)
        {
            return new WorkTask
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                ActualTimeCost = dto.ActualTimeCost,
                CreateDateTime = dto.CreateDateTime,
                PlanedTimeCost = dto.PlanedTimeCost,
                RemainingTimeCost = dto.RemainingTimeCost,
                PriorityId = dto.PriorityId,
                StateId = dto.StateId,
                UserId = dto.UserId,
                Version = dto.Version
            };
        }

        private TaskDto ToDto(WorkTask task)
        {
            return new TaskDto
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                ActualTimeCost = task.ActualTimeCost,
                CreateDateTime = task.CreateDateTime,
                PlanedTimeCost = task.PlanedTimeCost,
                RemainingTimeCost = task.RemainingTimeCost,
                PriorityId = task.PriorityId,
                StateId = task.StateId,
                UserId = task.UserId,
                Version = task.Version
            };
        }
    }
}