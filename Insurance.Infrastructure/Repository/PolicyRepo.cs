using AutoMapper;
using Insurance.Application.Common.Responses;
using Insurance.Application.DTOs.Policy;
using Insurance.Application.Interface;
using Insurance.Domain.Models;
using Insurance.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Insurance.Infrastructure.Repository
{
    public class PolicyRepo : IPolicyRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PolicyRepo(
            ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PolicyResponseDto>> CreatePolicyAsync(CreatePolicyRequestDto dto,int QuoteId)
        {
            var quote = await _context.Quotes
                .FirstOrDefaultAsync(x => x.QuoteId == QuoteId);

            if (quote == null)
            {
                return new ApiResponse<PolicyResponseDto>
                {
                    Success = false,
                    Message = "Quote not found"
                };
            }

            decimal finalPremium = quote.PremiumAmount;
            string status = "Active";

            if (quote.RiskCategory == "Medium")
            {
                finalPremium += quote.PremiumAmount * 0.10m;
            }
            else if (quote.RiskCategory == "High")
            {
                finalPremium += quote.PremiumAmount * 0.40m;
                status = "PendingApproval";
            }

            Policy policy = new Policy
            {
                QuoteId = quote.QuoteId,
                CustomerId = quote.CustomerId,
                ProductId = quote.ProductId,
                SumInsured = quote.SumInsured,
                PremiumAmount = finalPremium,
                PolicyStartDate = dto.PolicyStartDate,
                PolicyEndDate = dto.PolicyStartDate.AddYears(1),
                PolicyNumber = GeneratePolicyNumber(),
                Status = status,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = quote.CreatedBy
            };

            _context.Policies.Add(policy);

            await _context.SaveChangesAsync();

            //Log.Information("Policy Created {PolicyNumber}", policy.PolicyNumber);

            return new ApiResponse<PolicyResponseDto>
            {
                Success = true,
                Message = "Policy created successfully",
                Data = _mapper.Map<PolicyResponseDto>(policy)
            };
        }

        public async Task<ApiResponse<PolicyResponseDto>> GetPolicyByIdAsync(int policyId)
        {
            var policy = await _context.Policies
                .FirstOrDefaultAsync(x => x.PolicyId == policyId);

            if (policy == null)
            {
                return new ApiResponse<PolicyResponseDto>
                {
                    Success = false,
                    Message = "Policy not found"
                };
            }

            return new ApiResponse<PolicyResponseDto>
            {
                Success = true,
                Data = _mapper.Map<PolicyResponseDto>(policy)
            };
        }

        public async Task<ApiResponse<IEnumerable<PolicyResponseDto>>> GetCustomerPoliciesAsync(int customerId)
        {
            var policies = await _context.Policies
                .Where(x => x.CustomerId == customerId)
                .ToListAsync();

            return new ApiResponse<IEnumerable<PolicyResponseDto>>
            {
                Success = true,
                Data = _mapper.Map<IEnumerable<PolicyResponseDto>>(policies)
            };
        }

        public async Task<ApiResponse<IEnumerable<PolicyResponseDto>>> GetPendingPoliciesAsync(int pageNumber, int pageSize)
        {
            var policies = await _context.Policies
                .Where(x => x.Status == "PendingApproval")
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new ApiResponse<IEnumerable<PolicyResponseDto>>
            {
                Success = true,
                Data = _mapper.Map<IEnumerable<PolicyResponseDto>>(policies)
            };
        }

        public async Task<ApiResponse<string>> ApprovePolicyAsync(int policyId, int adminId)
        {
            var policy = await _context.Policies
                .FirstOrDefaultAsync(x => x.PolicyId == policyId);

            if (policy == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Policy not found"
                };
            }

            policy.Status = "Active";
            policy.ApprovedBy = adminId;
            policy.ApprovedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            //Log.Information("Policy Approved {PolicyId}", policyId);

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Policy approved successfully"
            };
        }

        public async Task<ApiResponse<string>> RejectPolicyAsync(int policyId, int adminId)
        {
            var policy = await _context.Policies
                .FirstOrDefaultAsync(x => x.PolicyId == policyId);

            if (policy == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Policy not found"
                };
            }

            policy.Status = "Rejected";
            policy.ApprovedBy = adminId;
            policy.ApprovedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            //log.Warning("Policy Rejected {PolicyId}", policyId);

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Policy rejected successfully"
            };
        }

        public async Task<ApiResponse<string>> CancelPolicyAsync(int policyId)
        {
            var policy = await _context.Policies
                .FirstOrDefaultAsync(x => x.PolicyId == policyId);

            if (policy == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Policy not found"
                };
            }

            policy.Status = "Cancelled";

            await _context.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Policy cancelled successfully"
            };
        }

        public async Task<ApiResponse<string>> RenewPolicyAsync(int policyId)
        {
            var policy = await _context.Policies
                .FirstOrDefaultAsync(x => x.PolicyId == policyId);

            if (policy == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Policy not found"
                };
            }

            policy.PolicyEndDate = policy.PolicyEndDate.AddYears(1);
            policy.Status = "Renewed";

            await _context.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Policy renewed successfully"
            };
        }

        private string GeneratePolicyNumber()
        {
            return $"POL-{DateTime.UtcNow:yyyyMMddHHmmss}";
        }
    }
}