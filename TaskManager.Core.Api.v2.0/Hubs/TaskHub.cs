using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using TaskManager.Core.Api.Models.Dto;

namespace TaskManager.Core.Api.Hubs
{
    public class TaskHub : Hub<ITaskHub>
    {
        public void CreateTask(TaskDto task)
        {
            Clients.AllExcept(new List<string> { this.Context.ConnectionId }).createTask(task);
        }

        public void EditTask(TaskDto task)
        {
            Clients.AllExcept(new List<string> { this.Context.ConnectionId }).editTask(task);
        }

        public void DeleteTask(TaskDto task)
        {
            Clients.AllExcept(new List<string> { this.Context.ConnectionId }).deleteTask(task);
        }

        //public void UserLogin(UserDto user)
        //{
        //    Groups.Add(Context.ConnectionId, user.Id);
        //    ConnectionCache.TryAdd(user.Token, Context.ConnectionId);
        //}

        //public void UserLogout(UserDto user)
        //{
        //    Groups.Remove(Context.ConnectionId, user.Id);
        //    string connection;
        //    ConnectionCache.TryRemove(user.Token, out connection);
        //}

        //public override Task OnConnected()
        //{
        //    return base.OnConnected();
        //}

    }
}
