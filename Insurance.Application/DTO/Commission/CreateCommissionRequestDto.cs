using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.DTO.Commission
{
    public class CreateCommissionRequestDto
    {
        public int AgentId { get; set; }

        public int ProductId { get; set; }

        public decimal CommissionPercentage { get; set; }
    }
}