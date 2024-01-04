using Application.Requests;

namespace Application.Services
{
    public interface IUserService
    {
        int CreateUser(UserRequest userRequest);

        void DeleteUser(int id);

        List<GetUserResponse> GetAllUsers();

        List<GetUserResponse> GetAllUsersByRole(int Role);

        GetUserResponse? GetUserByID(int id);

        GetUserResponse? GetUserByEmail(string email);

        GetUserResponse? GetUserByEmailAndPassword(string email, string password);

        void UpdateUser(UpdateUserRequest updateUserRequest);
    }
}
