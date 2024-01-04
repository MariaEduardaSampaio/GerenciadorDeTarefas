using Domain.AggregateObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        int CreateUser(UserModel userModel);
        List<UserModel> GetAllUsers();
        List<UserModel> GetAllUsersByRole(int Role);
        UserModel? GetUserByID(int id);

        UserModel? GetUserByEmail(string email);
        UserModel? GetUserByEmailAndPassword(string email, string password);
        void UpdateUser(UserModel userModel);
        void DeleteUser(UserModel userModel);
    }
}
