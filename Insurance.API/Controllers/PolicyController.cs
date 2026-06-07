using Asp.Versioning;
using Insurance.Application.DTOs.Policy;
using Insurance.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/policies")]
    [ApiVersion("1.0")]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyRepo db;

        public PolicyController(IPolicyRepo db)
        {
            this.db = db;
        }

        [HttpPost]
        [Route("CreatePolicy/{QuoteId}")]
        public async Task<IActionResult> CreatePolicy( CreatePolicyRequestDto dto, int QuoteId)
        {
            var result = await db.CreatePolicyAsync(dto, QuoteId);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetPolicyById/{policyId}")]
        public async Task<IActionResult> GetPolicyById(int policyId)
        {
            var result = await db.GetPolicyByIdAsync(policyId);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetCustomerPolicies/{customerId}")]
        public async Task<IActionResult> GetCustomerPolicies (int customerId)
        {
            var result = await db.GetCustomerPoliciesAsync(customerId);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetPendingPolicies")]
        public async Task<IActionResult> GetPendingPolicies (int pageNumber = 1, int pageSize = 10)
        {
            var result = await db.GetPendingPoliciesAsync(pageNumber, pageSize);
            return Ok(result);
        }


        [HttpPut]
        [Route("ApproveHighRiskQuote/{quoteId}")]
        public async Task<IActionResult> ApproveHighRiskQuote(int quoteId,int adminId)
        {
            var result = await db.ApproveHighRiskQuoteAsync(quoteId,adminId);

            return Ok(result);
        }


        [HttpPut]
        [Route("ApprovePolicy/{policyId}")]
        public async Task<IActionResult> ApprovePolicy ( int policyId,int adminId)
        {
            var result = await db.ApprovePolicyAsync(policyId, adminId);
            return Ok(result);
        }

        [HttpPut]
        [Route("RejectPolicy/{policyId}")]
        public async Task<IActionResult> RejectPolicy (int policyId,int adminId)
        {
            var result = await db.RejectPolicyAsync(policyId, adminId);
            return Ok(result);
        }

        [HttpPut]
        [Route("CancelPolicy/{policyId}")]
        public async Task<IActionResult> CancelPolicy (int policyId)
        {
            var result = await db.CancelPolicyAsync(policyId);
            return Ok(result);
        }

       
    }
}