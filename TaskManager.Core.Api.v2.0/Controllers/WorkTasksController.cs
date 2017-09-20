using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Api.Models.DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaskManager.Core.Api.Repository;
using System;
using TaskManager.Core.Api.Models.Dto;
using AutoMapper;

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
        public async Task<IEnumerable<TaskDto>> GetWorkTasks()
        {
            return Mapper.Map<IEnumerable<WorkTask>, IEnumerable<TaskDto>>(await _repository.Get().ConfigureAwait(false));
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

            return Ok(Mapper.Map<WorkTask, TaskDto>(workTask));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkTask([FromRoute] long id, [FromBody] TaskDto workTask)
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
                await _repository.Update(Mapper.Map<TaskDto, WorkTask>(workTask)).ConfigureAwait(false);
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
        public async Task<IActionResult> PostWorkTask([FromBody] TaskDto workTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            WorkTask task = await _repository.Create(Mapper.Map<TaskDto, WorkTask>(workTask)).ConfigureAwait(false);

            var dto = Mapper.Map<WorkTask, TaskDto>(task);
            return CreatedAtAction("GetWorkTask", new { id = dto.Id }, dto);
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

            return Ok(Mapper.Map<WorkTask, TaskDto>(workTask));
        }

        private async Task<bool> WorkTaskExists(long id)
        {
            return (await _repository.Get(id).ConfigureAwait(false)) != null;
        }
    }
}