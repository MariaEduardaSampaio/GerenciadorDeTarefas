using Domain.AggregateObjects;
using Domain.Interfaces;
using Infrastructure.Context;
using System.Data;


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

        public UserModel? GetUserByEmail(string email)
        {
            return _taskManagerContext.Users.FirstOrDefault(user => user.Email == email);
        }

        public UserModel? GetUserByEmailAndPassword(string email, string password)
        {
            return _taskManagerContext.Users.FirstOrDefault(user => user.Email == email && user.Password == password);
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
