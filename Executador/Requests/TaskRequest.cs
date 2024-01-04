namespace Application.Requests
{
    public class TaskRequest
    {
        public int Id { get; set; }
        public string? EmailResponsable { get; set; }
        public DateTime? EndDate { get; set; }
        public string Objective { get; set; }
        public string Description { get; set; }
    }
}
