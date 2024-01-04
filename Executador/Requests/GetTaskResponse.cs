using Domain.AggregateObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Requests
{
    public class GetTaskResponse
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public string? EmailResponsable { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Objective { get; set; }
        public string Description { get; set; }
    }
}
