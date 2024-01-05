using Application.Requests;
using Application.Requests.Enums;
using Domain.AggregateObjects;
using Domain.Interfaces;

namespace Application.Services
{
    internal class TaskService : ITaskService
    {
        private readonly ITasksRepository _tasksRepository;

        public TaskService(ITasksRepository taskRepository)
        {
            _tasksRepository = taskRepository;
        }


        public int CreateTask(TaskRequest taskRequest)
        {
            var task = new TaskModel()
            {
                EmailResponsable = taskRequest.EmailResponsable,
                EndDate = taskRequest.EndDate,
                Objective = taskRequest.Objective,
                Description = taskRequest.Description,
                CreatedDate = DateTime.Now,
                Status = (int)TaskStatusEnum.UnderAnalysis
            };

            _tasksRepository.CreateTask(task);
            return task.Id;
        }

        public void DeleteTask(int id)
        {
            var task = _tasksRepository.GetTaskByID(id);
            if (task != null)
                _tasksRepository.DeleteTask(task);
            else
                throw new ArgumentException("Não existe tarefa com este ID.");
        }

        public List<GetTaskResponse> GetAllTasks()
        {
            var tasks = _tasksRepository.GetAllTasks();
            return MapTasksToRequest(tasks);
        }
        private List<GetTaskResponse> MapTasksToRequest(List<TaskModel> tasks)
        {
            if (tasks.Any())
                return new List<GetTaskResponse>();

            return tasks.Select(task => new GetTaskResponse()
            {
                EmailResponsable = task.EmailResponsable,
                CreatedDate = DateTime.Now,
                EndDate = task.EndDate,
                Objective = task.Objective,
                Description = task.Description,
                Status = (TaskStatusEnum)task.Status
            }).ToList();
        }

        public List<GetTaskResponse> GetAllTasksByEmail(string email)
        {
            var tasks = _tasksRepository.GetAllTasksByEmail(email);
            return MapTasksToRequest(tasks);
        }

        public List<GetTaskResponse> GetAllTasksByObjective(string objective)
        {
            var tasks = _tasksRepository.GetAllTasksByEmail(objective);
            return MapTasksToRequest(tasks);
        }

        public GetTaskResponse? GetTaskByID(int id)
        {
            var task = _tasksRepository.GetTaskByID(id);
            return MapTaskToRequest(task);
        }

        private GetTaskResponse MapTaskToRequest(TaskModel? task)
        {
            if (task == null)
                return new GetTaskResponse();

            return new GetTaskResponse()
            {
                EmailResponsable = task.EmailResponsable,
                CreatedDate = DateTime.Now,
                EndDate = task.EndDate,
                Objective = task.Objective,
                Description = task.Description,
                Status = (TaskStatusEnum)task.Status
            };
        }

        public void UpdateTask(UpdateTaskRequest updateTaskRequest)
        {
            var taskModel = _tasksRepository.GetTaskByID(updateTaskRequest.Id);

            if (taskModel != null)
            {
                UpdateFieldsTasks(updateTaskRequest, taskModel);
                _tasksRepository.UpdateTask(taskModel);
            }
        }

        private void UpdateFieldsTasks(UpdateTaskRequest updateTaskRequest, TaskModel taskModel)
        {
            if (updateTaskRequest.EmailResponsable != null)
                taskModel.EmailResponsable = updateTaskRequest.EmailResponsable;
            if (updateTaskRequest.EndDate != null)
                taskModel.EndDate = updateTaskRequest.EndDate;
            if (updateTaskRequest.Objective != null)
                taskModel.Objective = updateTaskRequest.Objective;
            if (updateTaskRequest.Description != null)
                taskModel.Description = updateTaskRequest.Description;
            if (updateTaskRequest.Status != null)
                taskModel.Status = (int)updateTaskRequest.Status;
            if (updateTaskRequest.UserID != null)
                taskModel.UserID = updateTaskRequest.UserID;
        }
    }
}
