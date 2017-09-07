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
    [Route("api/priorities")]
    public class PrioritiesController : Controller
    {
        private readonly TaskDbContext _context;

        public PrioritiesController(TaskDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Priority> GetPriorities()
        {
            return _context.Priorities;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPriority([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var priority = await _context.Priorities.SingleOrDefaultAsync(m => m.Id == id);

            if (priority == null)
            {
                return NotFound();
            }

            return Ok(priority);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPriority([FromRoute] int id, [FromBody] Priority priority)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != priority.Id)
            {
                return BadRequest();
            }

            _context.Entry(priority).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PriorityExists(id))
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
        public async Task<IActionResult> PostPriority([FromBody] Priority priority)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Priorities.Add(priority);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPriority", new { id = priority.Id }, priority);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePriority([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var priority = await _context.Priorities.SingleOrDefaultAsync(m => m.Id == id);
            if (priority == null)
            {
                return NotFound();
            }

            _context.Priorities.Remove(priority);
            await _context.SaveChangesAsync();

            return Ok(priority);
        }

        private bool PriorityExists(int id)
        {
            return _context.Priorities.Any(e => e.Id == id);
        }
    }
}