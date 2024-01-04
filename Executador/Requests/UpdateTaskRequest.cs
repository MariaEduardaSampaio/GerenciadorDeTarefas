using Application.Requests.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Requests
{
    internal class UpdateTaskRequest
    {
        public int Id { get; set; }
        public string? EmailResponsable { get; set; }
        public TaskStatusEnum Status { get; set; }
        public DateTime? EndDate { get; set; }
        public string Objective { get; set; }
        public string Description { get; set; }
        public int UserID { get; set; }

    }
}
