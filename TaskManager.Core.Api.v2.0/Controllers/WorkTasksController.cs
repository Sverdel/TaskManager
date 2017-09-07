using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Api.Models.DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TaskManager.Core.Api.v2._0.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Route("api/tasks")]
    public class WorkTasksController : Controller
    {
        private readonly TaskDbContext _context;

        public WorkTasksController(TaskDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<WorkTask> GetWorkTasks()
        {
            return _context.WorkTasks;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkTask([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var workTask = await _context.WorkTasks.SingleOrDefaultAsync(m => m.Id == id);

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

            _context.Entry(workTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkTaskExists(id))
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

            _context.WorkTasks.Add(workTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorkTask", new { id = workTask.Id }, workTask);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkTask([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var workTask = await _context.WorkTasks.SingleOrDefaultAsync(m => m.Id == id);
            if (workTask == null)
            {
                return NotFound();
            }

            _context.WorkTasks.Remove(workTask);
            await _context.SaveChangesAsync();

            return Ok(workTask);
        }

        private bool WorkTaskExists(long id)
        {
            return _context.WorkTasks.Any(e => e.Id == id);
        }
    }
}