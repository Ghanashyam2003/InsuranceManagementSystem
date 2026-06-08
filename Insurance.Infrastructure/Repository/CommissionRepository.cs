using Insurance.Application.DTO.Commission;
using Insurance.Application.Interfaces;
using Insurance.Domain.Models;
using Insurance.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Repository
{
    public class CommissionRepository
        : ICommissionRepository
    {
        private readonly ApplicationDbContext db;

        public CommissionRepository(
            ApplicationDbContext db)
        {
            this.db = db;
        }

        private async Task CreateNotification(
    int userId,
    string message,
    string notificationType)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                NotificationType = notificationType,
                IsRead = false,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = 1
            };

            db.Notifications.Add(notification);

            await db.SaveChangesAsync();
        }

        public async Task GenerateCommission(
            int policyId,
            decimal premiumPaid)
        {
            var policy =
                await db.Policies
                    .FirstOrDefaultAsync(
                        x => x.PolicyId == policyId);

            if (policy == null)
                throw new Exception("Policy Not Found");

            if (policy.AgentId == null)
                throw new Exception(
                    "Policy not assigned to agent");

            var commissionRule =
                await db.AgentCommissions
                    .FirstOrDefaultAsync(x =>
                        x.AgentId == policy.AgentId &&
                        x.ProductId == policy.ProductId &&
                        x.Status == "Approved");

            if (commissionRule == null)
                throw new Exception(
                    "Approved commission rule not found");

            decimal commissionAmount =
                premiumPaid *
                commissionRule.CommissionPercentage
                / 100;

            var existingCommission =
                await db.Commissions
                    .FirstOrDefaultAsync(x =>
                        x.PolicyId == policyId &&
                        x.AgentId == policy.AgentId);

            if (existingCommission == null)
            {
                var commission =
                    new Commission
                    {
                        AgentId =
                            policy.AgentId.Value,

                        PolicyId =
                            policy.PolicyId,

                        CommissionAmount =
                            commissionAmount,

                        CommissionDate =
                            DateTime.UtcNow,

                        CreatedBy = 1
                    };

                db.Commissions.Add(
                    commission);
            }
            else
            {
                existingCommission
                    .CommissionAmount +=
                        commissionAmount;

                existingCommission
                    .ModifiedAt =
                        DateTime.UtcNow;

                existingCommission
                    .ModifiedBy = 1;
            }

            await db.SaveChangesAsync();
        }

        public async Task<List<CommissionResponseDto>>
            GetAgentCommissions(
                int agentId, int pageNumber,
                    int pageSize)
        {
            return await db.Commissions
                .Where(x =>
                    x.AgentId == agentId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x =>
                    new CommissionResponseDto
                    {
                        CommissionId =
                            x.CommissionId,

                        AgentId =
                            x.AgentId,

                        PolicyId =
                            x.PolicyId,

                        CommissionAmount =
                            x.CommissionAmount,

                        CommissionDate =
                            x.CommissionDate
                    })
                .ToListAsync();
        }

        public async Task<CommissionResponseDto?>
            GetPolicyCommission(
                int agentId,
                int policyId)
        {
            return await db.Commissions
                .Where(x =>
                    x.AgentId == agentId &&
                    x.PolicyId == policyId)
                .Select(x =>
                    new CommissionResponseDto
                    {
                        CommissionId =
                            x.CommissionId,

                        AgentId =
                            x.AgentId,

                        PolicyId =
                            x.PolicyId,

                        CommissionAmount =
                            x.CommissionAmount,

                        CommissionDate =
                            x.CommissionDate
                    })
                .FirstOrDefaultAsync();
        }


        public async Task<int> RequestCommission(
    CreateCommissionRequestDto dto)
        {
            var request =
                new AgentCommission
                {
                    AgentId = dto.AgentId,
                    ProductId = dto.ProductId,
                    CommissionPercentage =
                        dto.CommissionPercentage,

                    Status = "Pending",

                    CreatedAt = DateTime.UtcNow,

                    CreatedBy = dto.AgentId
                };

            db.AgentCommissions.Add(request);

            await db.SaveChangesAsync();

            await CreateNotification(
            1, // Admin User Id
            $"Agent {dto.AgentId} submitted a commission request.",
            "Commission Request");

            return request.AgentCommissionId;
        }


        public async Task<List<AgentCommission>>
    GetPendingCommissionRequests(int pageNumber,
                    int pageSize)
        {
            return await db.AgentCommissions
                .Where(x => x.Status == "Pending")
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public async Task ApproveCommissionRequest(
    int id)
        {
            var request =
                await db.AgentCommissions
                    .FirstOrDefaultAsync(x =>
                        x.AgentCommissionId == id);

            if (request == null)
                throw new Exception(
                    "Commission Request Not Found");

            request.Status = "Approved";

            await db.SaveChangesAsync();

            await CreateNotification(
            request.AgentId,
            $"Your commission request #{request.AgentCommissionId} has been approved.",
            "Commission Approved");
        }


        public async Task RejectCommissionRequest(
    int id)
        {
            var request =
                await db.AgentCommissions
                    .FirstOrDefaultAsync(x =>
                        x.AgentCommissionId == id);

            if (request == null)
                throw new Exception(
                    "Commission Request Not Found");

            request.Status = "Rejected";

            await db.SaveChangesAsync();

            await CreateNotification(
                request.AgentId,
                $"Your commission request #{request.AgentCommissionId} has been rejected.",
                "Commission Rejected");
        }

    }
}