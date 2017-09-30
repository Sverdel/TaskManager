using TaskManager.Core.Api.Models.Dto;

namespace TaskManager.Core.Api.Hubs
{
    public interface ITaskHub
    {
        void createTask(TaskDto task);
        void deleteTask(TaskDto task);
        void editTask(TaskDto task);
    }
}