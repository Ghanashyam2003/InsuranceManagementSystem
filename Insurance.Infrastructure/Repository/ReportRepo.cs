using Insurance.Application.DTO.Report;
using Insurance.Application.DTO.Report.Insurance.Application.DTO.Report;
using Insurance.Application.Interface;
using Insurance.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Repository
{
    public class ReportRepo : IReportRepo
    {
        ApplicationDbContext db;

        public ReportRepo(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<List<PolicyReportDto>>GetPolicyReportAsync()
        {
            return await db.Policies
                .Include(x => x.Customer)
                .Include(x => x.Product)
                .Include(x => x.Agent)
                .Select(x => new PolicyReportDto
                {
                    PolicyNumber = x.PolicyNumber,

                    CustomerName =
                        x.Customer.FirstName + " " +
                        x.Customer.LastName,

                    ProductName =
                        x.Product.ProductName,

                    AgentName =
                        x.Agent.AgentName,

                    PremiumAmount =
                        x.PremiumAmount,

                    Status =
                        x.Status,

                    PolicyStartDate =
                        x.PolicyStartDate,

                    PolicyEndDate =
                        x.PolicyEndDate
                })
                .ToListAsync();
        }
        public async Task<List<PremiumCollectionReportDto>>GetPremiumCollectionReportAsync()
        {
            return await db.PremiumSchedules
                .Include(x => x.Policy)
                .ThenInclude(x => x.Customer)
                .Select(x => new PremiumCollectionReportDto
                {
                    PolicyNumber =
                        x.Policy.PolicyNumber,

                    CustomerName =
                        x.Policy.Customer.FirstName + " " +
                        x.Policy.Customer.LastName,

                    InstallmentNumber =
                        x.InstallmentNumber,

                    Amount =
                        x.PremiumAmount,

                    DueDate =
                        x.DueDate,

                    PaidDate =
                        x.PaidDate,

                    IsPaid =
                        x.IsPaid
                })
                .ToListAsync();
        }
        public async Task<List<RevenueReportDto>>GetRevenueReportAsync()
        {
            return await db.PremiumSchedules
                .Where(x => x.IsPaid)
                .Include(x => x.Policy)
                .Select(x => new RevenueReportDto
                {
                    PolicyNumber =
                        x.Policy.PolicyNumber,

                    PremiumAmount =
                        x.PremiumAmount,

                    PaidDate =
                        x.PaidDate
                })
                .ToListAsync();
        }
        public async Task<List<AgentPerformanceReportDto>>GetAgentPerformanceReportAsync()
        {
            return await db.Policies
                .Include(x => x.Agent)
                .GroupBy(x => new
                {
                    x.AgentId,
                    x.Agent.AgentName
                })
                .Select(x => new AgentPerformanceReportDto
                {
                    AgentName =
                        x.Key.AgentName,

                    PoliciesSold =
                        x.Count(),

                    TotalPremium =
                        x.Sum(y => y.PremiumAmount)
                })
                .ToListAsync();
        }
        public async Task<List<ClaimReportDto>>GetClaimReportAsync()
        {
            return await db.Claims
                .Include(x => x.Policy)
                .Include(x => x.Agent)
                .Select(x => new ClaimReportDto
                {
                    ClaimId =
                        x.ClaimId,

                    PolicyNumber =
                        x.Policy.PolicyNumber,

                    AgentName =
                        x.Agent.AgentName,

                    ClaimAmount =
                        x.ClaimAmount,

                    ClaimReason =
                        x.ClaimReason,

                    ClaimDate =
                        x.ClaimDate,

                    Status =
                        x.Status
                })
                .ToListAsync();
        }
    }
}