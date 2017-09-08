using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Api.Models.DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaskManager.Core.Api.Repository;

namespace TaskManager.Core.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Route("api/tasks")]
    public class WorkTasksController : Controller
    {
        private readonly IRepository<WorkTask, long> _repository;

        public WorkTasksController(IRepository<WorkTask, long> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<WorkTask>> GetWorkTasks()
        {
            return await _repository.Get();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkTask([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var workTask = await _repository.Get(id);

            if (workTask == null)
            {
                return NotFound();
            }

            return Ok(workTask);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkTask([FromRoute] long id, [FromBody] WorkTask workTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != workTask.Id)
            {
                return BadRequest();
            }

            try
            {
                await _repository.Update(workTask);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await WorkTaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostWorkTask([FromBody] WorkTask workTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repository.Create(workTask);

            return CreatedAtAction("GetWorkTask", new { id = workTask.Id }, workTask);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkTask([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var workTask = await _repository.Get(id);
            if (workTask == null)
            {
                return NotFound();
            }

            await _repository.Delete(id);

            return Ok(workTask);
        }

        private async Task<bool> WorkTaskExists(long id)
        {
            return (await _repository.Get(id)) != null;
        }
    }
}