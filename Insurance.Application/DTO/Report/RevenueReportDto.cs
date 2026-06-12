using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.DTO.Report
{
    public class RevenueReportDto
    {
        public string? PolicyNumber { get; set; }

        public decimal PremiumAmount { get; set; }

        public DateTime? PaidDate { get; set; }
    }
}
