using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.DTO.Agent
{
    public class AgentDashboardDto
    {
        public int TotalCustomers { get; set; }

        public int TotalPolicies { get; set; }

        public int ActivePolicies { get; set; }

        public int PendingQuotes { get; set; }

        public int AcceptedQuotes { get; set; }

        public int RejectedQuotes { get; set; }

        public decimal TotalCommissionEarned { get; set; }

        public decimal CurrentMonthCommission { get; set; }

        public decimal TotalPremiumCollected { get; set; }

        public int ExpiringPoliciesNext30Days { get; set; }
    }
}
