using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using TaskManager.Api.Models.Dto;

namespace TaskManager.Api.Hubs
{
    public class TaskHub : Hub<ITaskHub>
    {
        public void CreateTask(TaskDto task)
        {
            Clients.AllExcept(new List<string> { Context.ConnectionId }).createTask(task);
        }

        public void EditTask(TaskDto task)
        {
            Clients.AllExcept(new List<string> { Context.ConnectionId }).editTask(task);
        }

        public void DeleteTask(TaskDto task)
        {
            Clients.AllExcept(new List<string> { Context.ConnectionId }).deleteTask(task);
        }
    }
}
