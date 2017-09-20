﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Api.Models.DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaskManager.Core.Api.Repository;
using System;

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
            return await _repository.Get().ConfigureAwait(false);
        }

        [HttpGet("list/{userId}")]
        public async Task<IActionResult> GetWorkTasks([FromRoute]string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _repository.Get(userId).ConfigureAwait(false));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkTask([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            WorkTask workTask = await _repository.Get(id).ConfigureAwait(false);

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
                await _repository.Update(workTask).ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await WorkTaskExists(id).ConfigureAwait(false))
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

            workTask.CreateDateTime = DateTime.Now;
            workTask.ChangeDatetime = DateTime.Now;
            WorkTask newTask = await _repository.Create(workTask).ConfigureAwait(false);

            return CreatedAtAction("GetWorkTask", new { id = newTask.Id }, newTask);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkTask([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            WorkTask workTask = await _repository.Get(id).ConfigureAwait(false);
            if (workTask == null)
            {
                return NotFound();
            }

            await _repository.Delete(id).ConfigureAwait(false);

            return Ok(workTask);
        }

        private async Task<bool> WorkTaskExists(long id)
        {
            return (await _repository.Get(id).ConfigureAwait(false)) != null;
        }
    }
}