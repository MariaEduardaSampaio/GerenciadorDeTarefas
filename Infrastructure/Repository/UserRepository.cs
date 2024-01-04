using Domain.AggregateObjects;
using Domain.Interfaces;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly TaskManagerContext _taskManagerContext;

        public UserRepository(TaskManagerContext taskManagerContext)
        {
            _taskManagerContext = taskManagerContext;
        }

        public int CreateUser(UserModel userModel)
        {
            _taskManagerContext.Users.Add(userModel);
            SaveChanges();
            return userModel.Id;
        }

        public void DeleteUser(UserModel userModel)
        {
            _taskManagerContext.Users.Remove(userModel);
            SaveChanges();
        }

        public List<UserModel> GetAllUsers()
        {
            return _taskManagerContext.Users.ToList();
        }

        public List<UserModel> GetAllUsersByRole(int Role)
        {
            return _taskManagerContext.Users.Where(user => user.Role == Role).ToList();
        }

        public UserModel? GetUserByID(int id)
        {
            return _taskManagerContext.Users.FirstOrDefault(user => user.Id == id);
        }

        public void UpdateUser(UserModel userModel)
        {
            _taskManagerContext.Users.Update(userModel);
            SaveChanges();
        }

        private void SaveChanges()
        {
            _taskManagerContext.SaveChanges();
        }
    }
}
