using Insurance.Application.Common.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insurance.Application.DTOs.Policy;

namespace Insurance.Application.Interfaces
{
    public interface IPolicyRepo
    {
        Task<ApiResponse<PolicyResponseDto>> CreatePolicyAsync(CreatePolicyRequestDto dto,int QuoteId);

            Task<ApiResponse<PolicyResponseDto>> GetPolicyByIdAsync(int policyId);

            Task<ApiResponse<IEnumerable<PolicyResponseDto>>> GetCustomerPoliciesAsync(int customerId);

            Task<ApiResponse<IEnumerable<PolicyResponseDto>>> GetPendingPoliciesAsync(int pageNumber, int pageSize);

            Task<ApiResponse<string>> ApproveHighRiskQuoteAsync(int quoteId,int adminId);

           Task<ApiResponse<string>> ApprovePolicyAsync(int policyId, int adminId);

            Task<ApiResponse<string>> RejectPolicyAsync(int policyId, int adminId);

            Task<ApiResponse<string>> CancelPolicyAsync(int policyId);
        }
    }
