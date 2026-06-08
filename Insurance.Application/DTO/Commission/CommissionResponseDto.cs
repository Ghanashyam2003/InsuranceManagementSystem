using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.DTO.Commission
{
    public class CommissionResponseDto
    {
        public int CommissionId { get; set; }

        public int AgentId { get; set; }

        public int PolicyId { get; set; }

        public decimal CommissionAmount { get; set; }

        public DateTime CommissionDate { get; set; }
    }
}
