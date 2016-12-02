using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using TaskManager.Api.Models.DataModel;
using System.Collections.Concurrent;

namespace TaskManager.Api.Models
{
    public class TaskHub : Hub
    {
        public static ConcurrentDictionary<string, string> ConnectionCache = new ConcurrentDictionary<string, string>();

        public void CreateTask(string key, WorkTask task)
        {
            Clients.Group(task.UserId.ToString(), ConnectionCache[key]).createTask(task);
        }

        public void EditTask(string key, WorkTask task)
        {
            Clients.Group(task.UserId.ToString(), ConnectionCache[key]).editTask(task);
        }

        public void DeleteTask(string key, WorkTask task)
        {
            Clients.Group(task.UserId.ToString(), ConnectionCache[key]).deleteTask(task);
        }

        public void UserLogin(User user)
        {
            Groups.Add(Context.ConnectionId, user.Id.ToString());
            ConnectionCache.TryAdd(user.Token, Context.ConnectionId);
        }

        public void UserLogout(User user)
        {
            Groups.Remove(Context.ConnectionId, user.Id.ToString());
            string connection;
            ConnectionCache.TryRemove(user.Token, out connection);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }
    }
}