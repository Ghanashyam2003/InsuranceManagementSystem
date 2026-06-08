using Insurance.Application.DTO.Commission;
using Insurance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.Interfaces
{
    public interface ICommissionRepository
    {
        Task GenerateCommission(
            int policyId,
            decimal premiumPaid);

        Task<List<CommissionResponseDto>>
                GetAgentCommissions(
                    int agentId,
                    int pageNumber,
                    int pageSize);

        Task<CommissionResponseDto?>
            GetPolicyCommission(
                int agentId,
                int policyId);



        Task<int> RequestCommission(
    CreateCommissionRequestDto dto);

        Task<List<AgentCommission>>
                GetPendingCommissionRequests(
                    int pageNumber,
                    int pageSize);

        Task ApproveCommissionRequest(int id);

        Task RejectCommissionRequest(int id);
    }
}