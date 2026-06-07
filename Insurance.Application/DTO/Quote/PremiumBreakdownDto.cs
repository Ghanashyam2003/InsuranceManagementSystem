using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.DTOs.Quote
{
    public class PremiumBreakdownDto
    {
        public int QuoteId { get; set; }
        public decimal SumInsured { get; set; }

        public decimal BaseRate { get; set; }
        public decimal BasePremium { get; set; }

        public int RiskScore { get; set; }
        public string RiskCategory { get; set; }

        public decimal RiskLoadingPercentage { get; set; }
        public decimal FinalPremium { get; set; }

        public string ProductType { get; set; }
    }
}