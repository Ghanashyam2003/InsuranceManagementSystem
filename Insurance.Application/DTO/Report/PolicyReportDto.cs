using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.DTO.Report
{
    namespace Insurance.Application.DTO.Report
    {
        public class PolicyReportDto
        {
            public string? PolicyNumber { get; set; }

            public string? CustomerName { get; set; }

            public string? ProductName { get; set; }

            public string? AgentName { get; set; }

            public decimal PremiumAmount { get; set; }

            public string? Status { get; set; }

            public DateTime PolicyStartDate { get; set; }

            public DateTime PolicyEndDate { get; set; }
        }
    }
}
