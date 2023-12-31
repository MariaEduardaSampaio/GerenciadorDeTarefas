﻿using Application.Requests.Enums;

namespace Application.Requests
{
    public class GetTaskResponse
    {
        public int Id { get; set; }
        public TaskStatusEnum Status { get; set; }
        public string? EmailResponsable { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Objective { get; set; }
        public string Description { get; set; }
        public int UserID { get; set; }

    }
}
