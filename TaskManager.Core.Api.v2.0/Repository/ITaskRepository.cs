using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Core.Api.Models.DataModel;

namespace TaskManager.Core.Api.Repository
{
    public interface ITaskRepository
    {
        Task CreateTask(WorkTask task);
        Task DeleteTask(long id);
        Task<WorkTask> GetTask(long id);
        Task<IEnumerable<WorkTask>> GetTasks();
        Task UpdateTask(WorkTask task);
    }
}