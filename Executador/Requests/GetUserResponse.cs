using Application.Requests.Enums;
using Application.Requests.ValueObjects;
using Domain.AggregateObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Requests
{
    public class GetUserResponse
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public SystemAccess AccessType { get; set; }
        public Role Role { get; set; }
        public Password Password { get; set; }
        public List<GetTaskResponse> Tasks { get; set; }
    }
}
