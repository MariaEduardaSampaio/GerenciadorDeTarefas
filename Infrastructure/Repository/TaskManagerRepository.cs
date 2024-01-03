using Domain.AggregateObjects;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace Infrastructure.Repository
{
    internal class TaskManagerRepository : ITaskManagerRepository
    {
        protected readonly TaskManagerContext _taskManagerContext;

        public TaskManagerRepository(TaskManagerContext taskManagerContext)
        {
            _taskManagerContext = taskManagerContext;
        }

        public int CreateTask(TaskModel taskModel)
        {
            _taskManagerContext.Tasks.Add(taskModel);
            _taskManagerContext.SaveChanges();
            return taskModel.Id;
        }

        public void DeleteTask(TaskModel taskModel)
        {
            _taskManagerContext.Tasks.Remove(taskModel);
        }

        public List<TaskModel> GetAllTasksByEmail(string email)
        {
            var tasks = _taskManagerContext.Tasks.Where(task => task.EmailResponsable == email).ToList();
            return tasks;
        }

        public List<TaskModel> GetAllTasksByObjective(string objective)
        {
            var tasks = _taskManagerContext.Tasks.Where(task => task.Objective == objective).ToList();
            return tasks;
        }

        public TaskModel GetTaskByID(int id)
        {
            var task = _taskManagerContext.Tasks.FirstOrDefault(task => task.Id == id);
            return task;
        }

        public void UpdateTask(TaskModel taskModel)
        {
            _taskManagerContext.Tasks.Update(taskModel);
        }
    }
}
