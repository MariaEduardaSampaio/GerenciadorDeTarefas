namespace Domain.AggregateObjects
{
    public class UserModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int AccessType { get; set; }
        public int Role { get; set; }
        public List<TaskModel> Tasks { get; set; }

    }
}
