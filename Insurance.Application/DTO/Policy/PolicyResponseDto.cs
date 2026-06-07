using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.DTOs.Policy
{
    public class PolicyResponseDto
    {
        public int PolicyId { get; set; }

        public string? PolicyNumber { get; set; }

        public int CustomerId { get; set; }

        public int ProductId { get; set; }

        public int? AgentId { get; set; }

        public decimal SumInsured { get; set; }

        public decimal PremiumAmount { get; set; }

        public string? RiskCategory { get; set; }

        public DateOnly PolicyStartDate { get; set; }

        public DateOnly PolicyEndDate { get; set; }

        public string? Status { get; set; }
    }
}