using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using TaskManager.Api.Models.DataModel;
using System.Collections.Concurrent;

namespace TaskManager.Api.Models
{
    public class TaskHub : Hub
    {

        public void CreateTask(string key, WorkTask task)
        {
            Clients.Group(task.UserId.ToString()).createTask(task);
        }

        public void EditTask(string key, WorkTask task)
        {
            Clients.Group(task.UserId.ToString()).editTask(task);
        }

        public void DeleteTask(string key, WorkTask task)
        {
            Clients.Group(task.UserId.ToString()).deleteTask(task);
        }

        public void UserLogin(User user)
        {
            Groups.Add(Context.ConnectionId, user.Id.ToString());
        }

        public void UserLogout(User user)
        {
            Groups.Remove(Context.ConnectionId, user.Id.ToString());
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }
    }
}