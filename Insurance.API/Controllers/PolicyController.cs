using Asp.Versioning;
using Insurance.Application.DTOs.Policy;
using Insurance.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/policies")]
    [ApiVersion("1.0")]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyRepo _policyService;

        public PolicyController(IPolicyRepo policyService)
        {
            _policyService = policyService;
        }

        [HttpPost("{QuoteId}")]
        public async Task<IActionResult> CreatePolicy(CreatePolicyRequestDto dto,int QuoteId)
        {
            var result = await _policyService.CreatePolicyAsync(dto, QuoteId);
            return Ok(result);
        }

        [HttpGet("{policyId}")]
        public async Task<IActionResult> GetPolicyById(int policyId)
        {
            var result = await _policyService.GetPolicyByIdAsync(policyId);
            return Ok(result);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCustomerPolicies(int customerId)
        {
            var result = await _policyService.GetCustomerPoliciesAsync(customerId);
            return Ok(result);
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingPolicies(
            int pageNumber = 1,
            int pageSize = 10)
        {
            var result = await _policyService.GetPendingPoliciesAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpPut("{policyId}/approve")]
        public async Task<IActionResult> ApprovePolicy(
            int policyId,
            int adminId)
        {
            var result = await _policyService.ApprovePolicyAsync(policyId, adminId);
            return Ok(result);
        }

        [HttpPut("{policyId}/reject")]
        public async Task<IActionResult> RejectPolicy(
            int policyId,
            int adminId)
        {
            var result = await _policyService.RejectPolicyAsync(policyId, adminId);
            return Ok(result);
        }

        [HttpPut("{policyId}/cancel")]
        public async Task<IActionResult> CancelPolicy(int policyId)
        {
            var result = await _policyService.CancelPolicyAsync(policyId);
            return Ok(result);
        }

        [HttpPut("{policyId}/renew")]
        public async Task<IActionResult> RenewPolicy(int policyId)
        {
            var result = await _policyService.RenewPolicyAsync(policyId);
            return Ok(result);
        }
    }
}