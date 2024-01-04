using Application.Requests.Enums;
using System;
using System.Collections.Generic;
namespace Application.Requests
{
    public class TaskRequest
    {
        public string? EmailResponsable { get; set; }
        public DateTime? EndDate { get; set; }
        public string Objective { get; set; }
        public string Description { get; set; }
    }
}
