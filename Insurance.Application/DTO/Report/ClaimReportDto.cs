using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.DTO.Report
{
    public class ClaimReportDto
    {
        public int ClaimId { get; set; }

        public string? PolicyNumber { get; set; }

        public string? AgentName { get; set; }

        public decimal ClaimAmount { get; set; }

        public string? ClaimReason { get; set; }

        public DateTime ClaimDate { get; set; }

        public string? Status { get; set; }
    }
}
