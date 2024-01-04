using Domain.AggregateObjects;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace Infrastructure.Repository
{
    public class TasksRepository : ITasksRepository
    {
        protected readonly TaskManagerContext _taskManagerContext;

        public TasksRepository(TaskManagerContext taskManagerContext)
        {
            _taskManagerContext = taskManagerContext;
        }

        public int CreateTask(TaskModel taskModel)
        {
            _taskManagerContext.Tasks.Add(taskModel);
            SaveChanges();
            return taskModel.Id;
        }

        public void DeleteTask(TaskModel taskModel)
        {
            _taskManagerContext.Tasks.Remove(taskModel);
            SaveChanges();
        }

        public List<TaskModel> GetAllTasks()
        {
            return _taskManagerContext.Tasks.ToList();
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

        public TaskModel? GetTaskByID(int id)
        {
            var task = _taskManagerContext.Tasks.FirstOrDefault(task => task.Id == id);
            return task;
        }

        public void UpdateTask(TaskModel taskModel)
        {
            _taskManagerContext.Tasks.Update(taskModel);
            SaveChanges();
        }

        private void SaveChanges()
        {
            _taskManagerContext.SaveChanges();
        }
    }
}
