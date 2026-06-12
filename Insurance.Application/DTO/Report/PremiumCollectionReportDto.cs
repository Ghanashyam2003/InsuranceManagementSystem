using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.DTO.Report
{
    public class PremiumCollectionReportDto
    {
        public string? PolicyNumber { get; set; }

        public string? CustomerName { get; set; }

        public int InstallmentNumber { get; set; }

        public decimal Amount { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? PaidDate { get; set; }

        public bool IsPaid { get; set; }
    }
}
