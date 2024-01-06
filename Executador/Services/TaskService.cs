using Application.Requests;
using Application.Requests.Enums;
using Domain.AggregateObjects;
using Domain.Interfaces;

namespace Application.Services
{
    internal class TaskService : ITaskService
    {
        private readonly ITasksRepository _tasksRepository;
        private readonly IUserRepository _userRepository;

        public TaskService(ITasksRepository taskRepository, IUserRepository userRepository)
        {
            _tasksRepository = taskRepository;
            _userRepository = userRepository;
        }


        public int CreateTask(TaskRequest taskRequest)
        {
            var user = _userRepository.GetUserByEmail(taskRequest.EmailResponsable!);
            var task = new TaskModel()
            {
                EmailResponsable = taskRequest.EmailResponsable,
                EndDate = taskRequest.EndDate,
                Objective = taskRequest.Objective,
                Description = taskRequest.Description,
                CreatedDate = DateTime.Now,
                Status = (int)TaskStatusEnum.UnderAnalysis,
                UserID = user!.Id
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
            if (tasks.Count == 0)
                return [];

            return tasks.Select(task => new GetTaskResponse()
            {
                Id = task.Id,
                EmailResponsable = task.EmailResponsable,
                CreatedDate = task.CreatedDate,
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
            var tasks = _tasksRepository.GetAllTasksByObjective(objective);
            return MapTasksToRequest(tasks);
        }

        public GetTaskResponse? GetTaskByID(int id)
        {
            var task = _tasksRepository.GetTaskByID(id);
            return MapTaskToRequest(task);
        }

        private GetTaskResponse? MapTaskToRequest(TaskModel? task)
        {
            if (task == null)
                return null;

            return new GetTaskResponse()
            {
                Id = task.Id,
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
            if (updateTaskRequest.Status != default)
                taskModel.Status = (int)updateTaskRequest.Status;
        }
    }
}
