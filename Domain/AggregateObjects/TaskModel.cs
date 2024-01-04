namespace Domain.AggregateObjects
{
    public class TaskModel
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public string? EmailResponsable { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Objective { get; set; }
        public string Description { get; set; }
        public UserModel User { get; set; }
        public int UserID { get; set; }
    }
}
