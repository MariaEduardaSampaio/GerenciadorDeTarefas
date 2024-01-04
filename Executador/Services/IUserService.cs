using Application.Requests;
using Application.Requests.Enums;
using Application.Requests.ValueObjects;
using Domain.AggregateObjects;
using Domain.Interfaces;

namespace Application.Services
{
    public interface IUserService
    {
        int CreateUser(UserRequest userRequest);

        void DeleteUser(int id);

        List<GetUserResponse> GetAllUsers();

        List<GetUserResponse> GetAllUsersByRole(int Role);

        GetUserResponse? GetUserByID(int id);

        void UpdateUser(UpdateUserRequest updateUserRequest);
    }
}
