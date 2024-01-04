using Domain.AggregateObjects;

namespace Domain.Interfaces
{
    public interface ITasksRepository
    {
        int CreateTask(TaskModel taskModel);
        TaskModel GetTaskByID(int id);
        public List<TaskModel> GetAllTasks();
        List<TaskModel> GetAllTasksByEmail(string email);
        List<TaskModel> GetAllTasksByObjective(string objective);
        void UpdateTask(TaskModel taskModel);
        void DeleteTask(TaskModel taskModel);
    }
}
