using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.DTOs.Policy
{
    public class PolicyMemberDto
    {
        public string? Name { get; set; }

        public string? Relation { get; set; }

        public DateTime DOB { get; set; }
    }
}