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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Route("api/states")]
    [ApiController]
    public class StatesController : Controller
    {
        private readonly IRepository<State, int> _repository;

        public StatesController(IRepository<State, int> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public Task<IEnumerable<State>> GetStates()
        {
            return _repository.Get();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<State>> GetState(int id)
        {
            var state = await _repository.Get(id).ConfigureAwait(false);

            if (state == null)
            {
                return NotFound();
            }

            return state;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<State>> PutState(int id, State state)
        {
            if (id != state.Id)
            {
                return BadRequest();
            }

            try
            {
                return await _repository.Update(state).ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! await StateExists(id).ConfigureAwait(false))
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
        public async Task<ActionResult<State>> PostState(State state)
        {
            var newState = await _repository.Create(state).ConfigureAwait(false);
            return CreatedAtAction("GetState", new { id = newState.Id }, newState);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<State>> DeleteState(int id)
        {
            var state = await _repository.Get(id).ConfigureAwait(false);
            if (state == null)
            {
                return NotFound();
            }

            await _repository.Delete(id).ConfigureAwait(false);
            return state;
        }

        private async Task<bool> StateExists(int id)
        {
            return (await _repository.Get(id).ConfigureAwait(false)) != null;
        }
    }
}