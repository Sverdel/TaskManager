using TaskManager.Api.Models.Dto;

namespace TaskManager.Api.Hubs
{
    public interface ITaskHub
    {
        void createTask(TaskDto task);
        void deleteTask(TaskDto task);
        void editTask(TaskDto task);
    }
}