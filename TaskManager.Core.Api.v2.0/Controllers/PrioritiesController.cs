using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaskManager.TaskCore.Repository;
using TaskManager.TaskCore.Model;

namespace TaskManager.Core.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/priorities")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PrioritiesController : Controller
    {
        private readonly IRepository<Priority, int> _repository;

        public PrioritiesController(IRepository<Priority, int> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<Priority>> GetPriorities()
        {
            return await _repository.Get().ConfigureAwait(false);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPriority([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Priority priority = await _repository.Get(id).ConfigureAwait(false);

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

            try
            {
                await _repository.Update(priority).ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! await PriorityExists(id).ConfigureAwait(false))
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

            Priority newPriority = await _repository.Create(priority).ConfigureAwait(false);

            return CreatedAtAction("GetPriority", new { id = newPriority.Id }, newPriority);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePriority([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Priority priority = await _repository.Get(id).ConfigureAwait(false);
            if (priority == null)
            {
                return NotFound();
            }

            await _repository.Delete(id).ConfigureAwait(false);

            return Ok(priority);
        }

        private async Task<bool> PriorityExists(int id)
        {
            return (await _repository.Get(id).ConfigureAwait(false)) != null;
        }
    }
}