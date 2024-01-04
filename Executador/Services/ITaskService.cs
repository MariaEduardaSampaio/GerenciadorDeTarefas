using Application.Requests;
using Domain.AggregateObjects;

namespace Application.Services
{
    internal interface ITaskService
    {
        int CreateTask(TaskRequest taskRequest);

        GetTaskResponse GetTaskByID(int id);

        public List<GetTaskResponse> GetAllTasks();

        List<GetTaskResponse> GetAllTasksByEmail(string email);

        List<GetTaskResponse> GetAllTasksByObjective(string objective);

        void UpdateTask(UpdateTaskRequest updateTaskRequest);

        void DeleteTask(int id);
    }
}
