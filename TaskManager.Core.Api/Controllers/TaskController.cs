﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TaskManager.Core.Api.Models;
using TaskManager.Core.Api.Models.DataModel;
using TaskManager.Core.Api.ViewModel;

namespace TaskManager.Core.Api.Controllers
{
    [Route("api/tasks/{userId}/{token}")]
    public class TaskController : Controller
    {
        private TaskDbContext _dbContext;

        public TaskController(TaskDbContext context, IConnectionManager manager)
        {
            _dbContext = context;
            //_hub = manager.GetHubContext<TaskHub>();
        }

        //private IHubContext _hub;

        [Authorize]
        [HttpGet]
        public IActionResult GetList(string userId, string token)
        {
            IEnumerable<TaskDto> test = _dbContext.WorkTasks.Where(x => x.UserId == userId).ToList().Select(x => new TaskDto { Id = x.Id, Name = x.Name });
            return Ok(test);
        }

        [Authorize]
        [HttpGet("{id:int}", Name = "GetTaskRoute")]
        public async Task<IActionResult> GetTask(int userId, string token, long id)
        {
            WorkTask workTask = await _dbContext.WorkTasks.FindAsync(id);
            if (workTask == null)
            {
                return NotFound();
            }

            return Ok(workTask);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostTask(string userId, string token, [FromBody]WorkTask task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_dbContext.WorkTasks.FirstOrDefault(x => x.UserId == userId && x.Name == task.Name) != null)
            {
                return BadRequest("Task with the same name already exists");
            }

            task.UserId = userId; 
            task.CreateDateTime = DateTime.Now;
            task.ChangeDatetime = DateTime.Now;

            _dbContext.WorkTasks.Add(task);
            await _dbContext.SaveChangesAsync();

            var newTask = await _dbContext.WorkTasks.FindAsync(task.Id);
            //_hub.Clients.Group(userId.ToString(), TaskHub.ConnectionCache[token]).createTask(newTask);

            return CreatedAtRoute("GetTaskRoute", new { userId, token, task.Id }, newTask);
        }

        [Authorize]
        [HttpPut()]
        public async Task<IActionResult> PutTask(string userId, string token, [FromBody]WorkTask task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if (id != task.Id)
            //{
            //    return BadRequest();
            //}

            task.ChangeDatetime = DateTime.Now;
            _dbContext.Entry(task).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
                //_hub.Clients.Group(userId.ToString(), TaskHub.ConnectionCache[token]).editTask(task);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_dbContext.WorkTasks.Count(e => e.Id == task.Id) == 0)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }


            return StatusCode((int)HttpStatusCode.NoContent);
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTask(string userId, string token, long id)
        {
            WorkTask workTask = await _dbContext.WorkTasks.FindAsync(id);
            if (workTask == null)
            {
                return NotFound();
            }

            _dbContext.WorkTasks.Remove(workTask);
            await _dbContext.SaveChangesAsync();

            //_hub.Clients.Group(userId.ToString(), TaskHub.ConnectionCache[token]).deleteTask(workTask);

            return Ok(new TaskDto { Id = workTask.Id, Name = workTask.Name });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}