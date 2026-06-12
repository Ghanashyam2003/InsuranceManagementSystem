using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.DTO.Agent
{
    public class CreateAgentDto
    {
        //public string AgentCode { get; set; }
        public string? AgentName { get; set; }
        public string? Email { get; set; }
        public string? MobileNumber { get; set; }
        public int CreatedBy { get; set; }
    }
}
