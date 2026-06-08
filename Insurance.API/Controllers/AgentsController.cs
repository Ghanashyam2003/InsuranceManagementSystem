using Asp.Versioning;
using Insurance.Application.Common.Responses;
using Insurance.Application.DTO.Agent;
using Insurance.Application.Interface;
using Insurance.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Insurance.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [EnableRateLimiting("ApiThrottle")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly IAgentRepository repo;
        private readonly ILogger<AgentsController> logger;

        public AgentsController(
    IAgentRepository repo,
    ILogger<AgentsController> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAgent(CreateAgentDto dto)
        {
            var id = await repo.CreateAgent(dto);
            logger.LogInformation(
    "Creating agent {AgentName}",
    dto.AgentName);

            return Ok(new
            {
                Success = true,
                Message = "Agent created successfully",
                AgentId = id
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber,
    int pageSize)
        {
            var result = await repo.GetAllAgents(pageNumber,
    pageSize);
            logger.LogInformation(
    "Fetching all agents");
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await repo.GetAgentById(id);

            if (result == null)
                return NotFound("Agent not found");

            logger.LogInformation(
    "Fetching agent {AgentId}",
    id);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateAgentDto dto)
        {
            await repo.UpdateAgent(id, dto);

            logger.LogInformation(
    "Updating agent {AgentId}",
    id);

            return Ok(new
            {
                Success = true,
                Message = "Agent updated successfully"
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await repo.DeleteAgent(id);

            logger.LogInformation(
    "Deleting agent {AgentId}",
    id);

            return Ok(new
            {
                Success = true,
                Message = "Agent deleted successfully"
            });
        }

        [HttpGet("{id}/policies")]
        public async Task<IActionResult> GetAgentPolicies(int id, int pageNumber,
    int pageSize)
        {
            var result =
                await repo.GetAgentPolicies(id, pageNumber,
     pageSize);

            logger.LogInformation(
    "Fetching policies for AgentId {AgentId}",
    id);

            return Ok(
                ResponseHelper.Success(
                    result,
                    "Policies Retrieved Successfully"));
        }

        [HttpGet("{id}/customers")]
        public async Task<IActionResult> GetAgentCustomers(int id, int pageNumber,
    int pageSize)
        {
            var result =
                await repo.GetAgentCustomers(id, pageNumber,
    pageSize);

            logger.LogInformation(
    "Fetching customers for AgentId {AgentId}",
    id);

            return Ok(
                ResponseHelper.Success(
                    result,
                    "Customers Retrieved Successfully"));
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchAgents([FromQuery] string name , int pageNumber,
    int pageSize)
        {
            var result =
                await repo.SearchAgents(name, pageNumber,
    pageSize);

            logger.LogInformation(
    "Searching agents with name {Name}",
    name);

            return Ok(
                ResponseHelper.Success(
                    result,
                    "Agents Retrieved Successfully"));
        }

        [HttpGet("{id}/customers/pending-quotes")]
        public async Task<IActionResult>GetCustomersWithPendingQuotes(int id, int pageNumber,
    int pageSize)
        {
            var result =
                await repo
                    .GetCustomersWithPendingQuotes(id,pageNumber,
    pageSize);

            logger.LogInformation(
    "Fetching pending quote customers for AgentId {AgentId}",
    id);

            return Ok(
                ResponseHelper.Success(
                    result,
                    "Pending Quote Customers Retrieved Successfully"));
        }

        [HttpGet("{id}/customers/active-policies")]
        public async Task<IActionResult>GetCustomersWithActivePolicies(int id, int pageNumber,
    int pageSize)
        {
            var result =
                await repo
                    .GetCustomersWithActivePolicies(id,pageNumber,
    pageSize);

            logger.LogInformation(
    "Fetching pending quote customers for AgentId {AgentId}",
    id);

            return Ok(
                ResponseHelper.Success(
                    result,
                    "Active Policy Customers Retrieved Successfully"));
        }


        [HttpGet("{id}/dashboard")]
        public async Task<IActionResult> GetDashboard(int id)
        {
            var result =
                await repo.GetDashboard(id);

            logger.LogInformation(
    "Fetching dashboard for AgentId {AgentId}",
    id);

            return Ok(
                ResponseHelper.Success(
                    result,
                    "Dashboard Retrieved Successfully"));
        }
    }
}