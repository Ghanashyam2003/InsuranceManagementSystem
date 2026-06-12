using Asp.Versioning;
using Insurance.Application.Common.Responses;
using Insurance.Application.DTO.Commission;
using Insurance.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Insurance.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [EnableRateLimiting("ApiThrottle")]
    [ApiController]
    public class CommissionController
        : ControllerBase
    {
        private readonly
            ICommissionRepository repo;

        private readonly ILogger<CommissionController> logger;

        public CommissionController(
    ICommissionRepository repo,
    ILogger<CommissionController> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }

        [HttpPost("generate")]
        public async Task<IActionResult>
            GenerateCommission(
                GenerateCommissionDto dto)
        {
            await repo.GenerateCommission(
                dto.PolicyId,
                dto.PremiumPaid);

            logger.LogInformation(
    "Generating commission for PolicyId {PolicyId}",
    dto.PolicyId);

            return Ok(
                ResponseHelper.Success<object>(
                    null,
                    "Commission Generated Successfully"));
        }

        [HttpGet("agent/{agentId}")]
        public async Task<IActionResult>
GetAgentCommissions(
    int agentId,
    int pageNumber = 1,
    int pageSize = 10)
        {
            var result =
                await repo.GetAgentCommissions(
                    agentId,
                    pageNumber,
                    pageSize);

            logger.LogInformation(
                "Fetching commissions for AgentId {AgentId}",
                agentId);

            return Ok(
                ResponseHelper.Success(
                    result,
                    "Commissions Retrieved Successfully"));
        }

        [HttpGet(
            "agent/{agentId}/policy/{policyId}")]
        public async Task<IActionResult>
            GetPolicyCommission(
                int agentId,
                int policyId)
        {
            var result =
                await repo
                    .GetPolicyCommission(
                        agentId,
                        policyId);

            logger.LogInformation(
    "Fetching commission for AgentId {AgentId} PolicyId {PolicyId}",
    agentId,
    policyId);

            return Ok(
                ResponseHelper.Success(
                    result,
                    "Commission Retrieved Successfully"));
        }


        [HttpPost("request")]
        public async Task<IActionResult>
    RequestCommission(
        CreateCommissionRequestDto dto)
        {
            var id =
                await repo.RequestCommission(dto);

            logger.LogInformation(
    "Commission request submitted by AgentId {AgentId}",
    dto.AgentId);

            return Ok(
                ResponseHelper.Success(
                    id,
                    "Commission Request Submitted Successfully"));
        }

        [HttpGet("pending")]
        public async Task<IActionResult>
GetPendingRequests(
    int pageNumber = 1,
    int pageSize = 10)
        {
            var result =
                await repo.GetPendingCommissionRequests(
                    pageNumber,
                    pageSize);

            logger.LogInformation(
                "Fetching pending commission requests");

            return Ok(
                ResponseHelper.Success(
                    result,
                    "Pending Requests Retrieved Successfully"));
        }

        [HttpPut("{id}/approve")]
        public async Task<IActionResult>
    ApproveRequest(
        int id)
        {
            await repo
                .ApproveCommissionRequest(id);

            logger.LogInformation(
    "Approving commission request {RequestId}",
    id);

            return Ok(
                ResponseHelper.Success<object>(
                    null,
                    "Commission Request Approved"));
        }

        [HttpPut("{id}/reject")]
        public async Task<IActionResult>
    RejectRequest(
        int id)
        {
            await repo
                .RejectCommissionRequest(id);

            logger.LogInformation(
    "Rejecting commission request {RequestId}",
    id);

            return Ok(
                ResponseHelper.Success<object>(
                    null,
                    "Commission Request Rejected"));
        }


    }
}