using Domain.AggregateObjects;

namespace Domain.Interfaces
{
    public interface ITaskManagerRepository
    {
        int CreateTask(TaskModel taskModel);
        TaskModel GetTaskByID(int id);
        List<TaskModel> GetAllTasksByEmail(string email);
        List<TaskModel> GetAllTasksByObjective(string objective);
        void UpdateTask(TaskModel taskModel);
        void DeleteTask(TaskModel taskModel);
    }
}
