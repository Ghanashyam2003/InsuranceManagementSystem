using Insurance.Application.DTO.Report;
using Insurance.Application.DTO.Report.Insurance.Application.DTO.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.Interface
{
    public interface IReportRepo
    {
        Task<List<PolicyReportDto>>GetPolicyReportAsync();

        Task<List<PremiumCollectionReportDto>>GetPremiumCollectionReportAsync();

        Task<List<RevenueReportDto>>GetRevenueReportAsync();

        Task<List<AgentPerformanceReportDto>>GetAgentPerformanceReportAsync();

        Task<List<ClaimReportDto>>GetClaimReportAsync();
    }
}
