using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using TaskManager.Api.Models.DataModel;
using TaskManager.Api.Models.Dto;
using System.Collections.Concurrent;

namespace TaskManager.Api.Models
{
    public class TaskHub : Hub
    {

        public void CreateTask(string key, TaskDto task)
        {
            Clients.Group(task.UserId.ToString()).createTask(task);
        }

        public void EditTask(string key, TaskDto task)
        {
            Clients.Group(task.UserId.ToString()).editTask(task);
        }

        public void DeleteTask(string key, TaskDto task)
        {
            Clients.Group(task.UserId.ToString()).deleteTask(task);
        }

        public void UserLogin(UserDto user)
        {
            Groups.Add(Context.ConnectionId, user.Id.ToString());
        }

        public void UserLogout(UserDto user)
        {
            Groups.Remove(Context.ConnectionId, user.Id.ToString());
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }
    }
}