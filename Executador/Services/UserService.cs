using Application.Requests;
using Application.Requests.Enums;
using Application.Requests.ValueObjects;
using Domain.AggregateObjects;
using Domain.Interfaces;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public int CreateUser(UserRequest userRequest)
        {
            var user = new UserModel()
            {
                Name = userRequest.Name,
                Email = userRequest.Email,
                Password = userRequest.Password.password,
                Role = (int)userRequest.Role,
                AccessType = ConfigureAccessType(userRequest.Role),
                Tasks = MapTasksToModel(userRequest.Tasks)
            };

            _userRepository.CreateUser(user);
            return user.Id;
        }

        private List<TaskModel> MapTasksToModel(List<TaskRequest> tasks)
        {
            if (tasks.Any())
                return new List<TaskModel>();

            return tasks.Select(task => new TaskModel()
            {
                EmailResponsable = task.EmailResponsable,
                Objective = task.Objective,
                Description = task.Description,
                CreatedDate = DateTime.Now,
                Status = (int)TaskStatusEnum.UnderAnalysis,
                EndDate = task.EndDate
            }).ToList();
        }

        private int ConfigureAccessType(Role role)
        {
            if (role == Role.Developer)
                return (int)SystemAccess.PARCIAL;
            else
                return (int)SystemAccess.TOTAL;
        }

        public void DeleteUser(int id)
        {
            var user = _userRepository.GetUserByID(id);
            if (user != null)
                _userRepository.DeleteUser(user);
            else
                throw new ArgumentException("Não existe usuário com este ID.");
        }

        public List<GetUserResponse> GetAllUsers()
        {
            var users = _userRepository.GetAllUsers();
            return MapUsersToRequest(users);
        }

        private List<GetUserResponse> MapUsersToRequest(List<UserModel> users)
        {
            if (users.Any())
                return new List<GetUserResponse>();

            return users.Select(user => new GetUserResponse()
            {
                Email = user.Email,
                Name = user.Name,
                Role = (Role)user.Role,
                Password = new Password(user.Password),
                Tasks = MapTasksToRequest(user.Tasks)
            }).ToList();
        }
        private GetUserResponse? MapUserToRequest(UserModel user)
        {
            if (user == null)
                return null;

            return new GetUserResponse()
            {
                Email = user.Email,
                Name = user.Name,
                Role = (Role)user.Role,
                Password = new Password(user.Password!),
                Tasks = MapTasksToRequest(user.Tasks)
            };
        }

        private List<GetTaskResponse> MapTasksToRequest(List<TaskModel> tasks)
        {
            if (tasks.Any())
                return new List<GetTaskResponse>();

            return tasks.Select(task => new GetTaskResponse()
            {
                EmailResponsable = task.EmailResponsable,
                Objective = task.Objective,
                Description = task.Description,
                CreatedDate = DateTime.Now,
                Status = TaskStatusEnum.UnderAnalysis,
                EndDate = task.EndDate
            }).ToList();
        }

        public List<GetUserResponse> GetAllUsersByRole(int Role)
        {
            var users = _userRepository.GetAllUsersByRole(Role);
            return MapUsersToRequest(users);
        }

        public GetUserResponse? GetUserByID(int id)
        {
            var user = _userRepository.GetUserByID(id);
            return MapUserToRequest(user);
        }
        public GetUserResponse? GetUserByEmail(string email)
        {
            var user = _userRepository.GetUserByEmail(email);
            return MapUserToRequest(user);
        }
        public GetUserResponse? GetUserByEmailAndPassword(string email, string password)
        {
            var user = _userRepository.GetUserByEmailAndPassword(email, password);
            return MapUserToRequest(user);
        }

        public void UpdateUser(UpdateUserRequest updateUserRequest)
        {
            var userModel = _userRepository.GetUserByID(updateUserRequest.Id);

            if (userModel != null)
            {
                UpdateFieldsUser(updateUserRequest, userModel);
                _userRepository.UpdateUser(userModel);
            }
        }

        private void UpdateFieldsUser(UpdateUserRequest updateUserRequest, UserModel userModel)
        {
            if (updateUserRequest.Name != null)
                userModel.Name = updateUserRequest.Name;
            if (updateUserRequest.Email != null)
                userModel.Email = updateUserRequest.Email;
            if (updateUserRequest.Password != null)
                userModel.Password = updateUserRequest.Password.password;
            if (updateUserRequest.Tasks != null)
                userModel.Tasks = MapTasksToModel(updateUserRequest.Tasks);
            if (updateUserRequest.Role != null)
            {
                userModel.Role = (int)updateUserRequest.Role;
                userModel.AccessType = ConfigureAccessType((Role)userModel.Role);
            }
        }

    }
}
