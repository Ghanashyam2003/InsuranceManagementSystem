using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.DTO.Report
{
    public class AgentPerformanceReportDto
    {
        public string? AgentName { get; set; }

        public int PoliciesSold { get; set; }

        public decimal TotalPremium { get; set; }
    }
}