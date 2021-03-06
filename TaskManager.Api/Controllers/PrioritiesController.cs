﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaskManager.Core.Repository;
using TaskManager.Core.Model;

namespace TaskManager.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/priorities")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PrioritiesController : Controller
    {
        private readonly IRepository<Priority, int> _repository;

        public PrioritiesController(IRepository<Priority, int> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public Task<IEnumerable<Priority>> GetPriorities()
        {
            return _repository.Get();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Priority>> GetPriority(int id)
        {
            var priority = await _repository.Get(id).ConfigureAwait(false);

            if (priority == null)
            {
                return NotFound();
            }

            return priority;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Priority>> PutPriority(int id, Priority priority)
        {
            if (id != priority.Id)
            {
                return BadRequest();
            }

            try
            {
                return await _repository.Update(priority).ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PriorityExists(id).ConfigureAwait(false))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult<Priority>> PostPriority(Priority priority)
        {
            var newPriority = await _repository.Create(priority).ConfigureAwait(false);

            return CreatedAtAction("GetPriority", new { id = newPriority.Id }, newPriority);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Priority>> DeletePriority(int id)
        {
            var priority = await _repository.Get(id).ConfigureAwait(false);
            if (priority == null)
            {
                return NotFound();
            }

            await _repository.Delete(id).ConfigureAwait(false);

            return priority;
        }

        private async Task<bool> PriorityExists(int id)
        {
            return (await _repository.Get(id).ConfigureAwait(false)) != null;
        }
    }
}