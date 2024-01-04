using Application.Requests.Enums;
using Application.Requests.ValueObjects;

namespace Application.Requests
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Role? Role { get; set; }
        public Password Password { get; set; }
        public List<TaskRequest> Tasks { get; set; } = new List<TaskRequest>();
    }
}
