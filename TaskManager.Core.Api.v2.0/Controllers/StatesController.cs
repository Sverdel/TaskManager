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
    [Route("api/states")]
    public class StatesController : Controller
    {
        private readonly IRepository<State, int> _repository;

        public StatesController(IRepository<State, int> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<State>> GetStates()
        {
            return await _repository.Get();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetState([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var state = await _repository.Get(id);

            if (state == null)
            {
                return NotFound();
            }

            return Ok(state);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutState([FromRoute] int id, [FromBody] State state)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != state.Id)
            {
                return BadRequest();
            }

            try
            {
                await _repository.Update(state);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! await StateExists(id))
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
        public async Task<IActionResult> PostState([FromBody] State state)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repository.Create(state);
            return CreatedAtAction("GetState", new { id = state.Id }, state);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteState([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var state = await _repository.Get(id);
            if (state == null)
            {
                return NotFound();
            }

            await _repository.Delete(id);
            return Ok(state);
        }

        private async Task<bool> StateExists(int id)
        {
            return (await _repository.Get(id)) != null;
        }
    }
}