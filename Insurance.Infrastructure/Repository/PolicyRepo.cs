using AutoMapper;
using Insurance.Application.Common.Responses;
using Insurance.Application.DTOs.Policy;
using Insurance.Application.Interfaces;
using Insurance.Domain.Models;
using Insurance.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Repository
{
    public class PolicyRepo : IPolicyRepo
    {
        ApplicationDbContext db;
        IMapper mapper;

        public PolicyRepo(ApplicationDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<PolicyResponseDto>> CreatePolicyAsync (CreatePolicyRequestDto dto,int QuoteId)
        {
            var quote = await db.Quotes.FirstOrDefaultAsync(x => x.QuoteId == QuoteId);

            if (quote == null)
            {
                throw new Exception("Quote Not Found");
            }

            if (quote.RiskCategory == "High" && quote.Status != "ApprovedForPolicy")
            {
                throw new Exception("High Risk Quote requires Admin Approval");
            }

            if (quote.Status != "Accepted" && quote.Status != "ApprovedForPolicy")
            {
                throw new Exception(
                    "Quote is not eligible for policy creation");
            }
            //if (quote.Status != "Accepted")
            //{
            //    throw new Exception("Policy can only be created for Accepted Quotes");
            //}

            // One Quote = One Policy
            var existingPolicy = await db.Policies .FirstOrDefaultAsync(x => x.QuoteId == QuoteId);

            if (existingPolicy != null)
            {
                throw new Exception("Policy already exists for this Quote");
            }

            decimal finalPremium = quote.PremiumAmount;
            string status = "PendingPayment";

            if (quote.RiskCategory == "Medium")
            {
                finalPremium += quote.PremiumAmount * 0.10m;
            }
            else if (quote.RiskCategory == "High")
            {
                finalPremium += quote.PremiumAmount * 0.40m;
            } 

            Policy policy = new Policy
            {
                QuoteId = quote.QuoteId,
                CustomerId = quote.CustomerId,
                ProductId = quote.ProductId,
                SumInsured = quote.SumInsured,
                PremiumAmount = finalPremium,
                PolicyStartDate = dto.PolicyStartDate.ToDateTime(TimeOnly.MinValue),
                PolicyEndDate = dto.PolicyStartDate.AddYears(quote.PolicyTenureYears).ToDateTime(TimeOnly.MinValue),
                PolicyNumber = await GeneratePolicyNumber(),
                Status = status,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = quote.CreatedBy
            };

            await db.Policies.AddAsync(policy);
            await db.SaveChangesAsync();

            return new ApiResponse<PolicyResponseDto>
            {
                Success = true,
                Message = "Policy Created Successfully",
                Data = mapper.Map<PolicyResponseDto>(policy)
            };
        }


        public async Task<ApiResponse<string>> ApproveHighRiskQuoteAsync(int quoteId,int adminId)
        {
            var quote = await db.Quotes
                .FirstOrDefaultAsync(x => x.QuoteId == quoteId);

            if (quote == null)
            {
                throw new Exception("Quote Not Found");
            }

            quote.Status = "ApprovedForPolicy";

            quote.ModifiedBy = adminId;
            quote.ModifiedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = "High Risk Quote Approved Successfully"
            };
        }


        public async Task<ApiResponse<PolicyResponseDto>> GetPolicyByIdAsync(int policyId)
        {
            var policy = await db.Policies.FirstOrDefaultAsync(x => x.PolicyId == policyId);

            if (policy == null)
            {
                throw new Exception("Policy Not Found");
            }

            return new ApiResponse<PolicyResponseDto>
            {
                Success = true,
                Data = mapper.Map<PolicyResponseDto>(policy)
            };
        }

        public async Task<ApiResponse<IEnumerable<PolicyResponseDto>>> GetCustomerPoliciesAsync (int customerId)
        {
            var data = await db.Policies.Where(x => x.CustomerId == customerId).ToListAsync();

            return new ApiResponse<IEnumerable<PolicyResponseDto>>
            {
                Success = true,
                Data = mapper.Map<IEnumerable<PolicyResponseDto>>(data)
            };
        }

        public async Task<ApiResponse<IEnumerable<PolicyResponseDto>>> GetPendingPoliciesAsync (int pageNumber,int pageSize)
        {
            var data = await db.Policies.Where(x => x.Status == "PendingApproval")
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new ApiResponse<IEnumerable<PolicyResponseDto>>
            {
                Success = true,
                Data = mapper.Map<IEnumerable<PolicyResponseDto>>(data)
            };
        }

        public async Task<ApiResponse<string>> ApprovePolicyAsync(int policyId,int adminId)
        {
            var data = await db.Policies.FirstOrDefaultAsync(x => x.PolicyId == policyId);

            if (data == null)
            {
                throw new Exception("Policy Not Found");
            }

            data.Status = "Active";
            data.ApprovedBy = adminId;
            data.ApprovedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Policy Approved Successfully"
            };
        }

        public async Task<ApiResponse<string>> RejectPolicyAsync(int policyId,int adminId)
        {
            var data = await db.Policies.FirstOrDefaultAsync(x => x.PolicyId == policyId);

            if (data == null)
            {
                throw new Exception("Policy Not Found");
            }

            data.Status = "Rejected";
            data.ApprovedBy = adminId;
            data.ApprovedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Policy Rejected Successfully"
            };
        }

        public async Task<ApiResponse<string>> CancelPolicyAsync(int policyId)
        {
            var data = await db.Policies.FirstOrDefaultAsync(x => x.PolicyId == policyId);

            if (data == null)
            {
                throw new Exception("Policy Not Found");
            }

            data.Status = "Cancelled";

            await db.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Policy Cancelled Successfully"
            };
        }

       

        private async Task<string> GeneratePolicyNumber()
        {
            int nextNumber = 1001;

            var lastPolicy = await db.Policies.OrderByDescending(x => x.PolicyId).FirstOrDefaultAsync();

            if (lastPolicy != null)
            {
                nextNumber = 1000 + lastPolicy.PolicyId + 1;
            }

            return $"POL{nextNumber}";
        }
    }
}