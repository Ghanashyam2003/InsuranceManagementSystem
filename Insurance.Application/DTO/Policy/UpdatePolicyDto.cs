using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.DTOs.Policy
{
    public class UpdatePolicyDto
    {
        public DateTime PolicyStartDate { get; set; }

        public DateTime PolicyEndDate { get; set; }

        public decimal SumInsured { get; set; }
    }
}