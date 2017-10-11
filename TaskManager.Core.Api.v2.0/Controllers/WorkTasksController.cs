﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Core.Api.Hubs;
using TaskManager.Core.Api.Models.Dto;
using TaskManager.TaskCore.Model;
using TaskManager.TaskCore.Repository;

namespace TaskManager.Core.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Route("api/tasks")]
    public class WorkTasksController : Controller
    {
        private readonly IRepository<WorkTask, long> _repository;
        private readonly IHubContext<TaskHub> _taskHub;

        public WorkTasksController(IRepository<WorkTask, long> repository, IHubContext<TaskHub> taskHub)
        {
            _repository = repository;
            _taskHub = taskHub;
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
                WorkTask task = await _repository.Update(Mapper.Map<TaskDto, WorkTask>(workTask)).ConfigureAwait(false);
                await _taskHub.Clients.All.InvokeAsync("editTask", Mapper.Map<WorkTask, TaskDto>(task)).ConfigureAwait(false);
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

            TaskDto dto = Mapper.Map<WorkTask, TaskDto>(task);
            await _taskHub.Clients.All.InvokeAsync("createTask", dto).ConfigureAwait(false);
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

            TaskDto dto = Mapper.Map<WorkTask, TaskDto>(workTask);
            await _taskHub.Clients.All.InvokeAsync("deleteTask", dto).ConfigureAwait(false);
            return Ok(dto);
        }

        private async Task<bool> WorkTaskExists(long id)
        {
            return (await _repository.Get(id).ConfigureAwait(false)) != null;
        }
    }
}