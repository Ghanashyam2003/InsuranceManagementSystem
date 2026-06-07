using Insurance.Application.DTOs;
using Insurance.Application.Interfaces;
using Insurance.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Insurance.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgentController : ControllerBase
    {
        private readonly IAgentService    agentService;
        private readonly ICustomerService customerService;
        private readonly ApplicationDbContext db;

        public AgentController(
            IAgentService agentService,
            ICustomerService customerService,
            ApplicationDbContext db)
        {
            this.agentService    = agentService;
            this.customerService = customerService;
            this.db              = db;
        }

        /// <summary>[Admin] Create a new Agent. Password is auto-generated and emailed.</summary>
        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAgent([FromBody] AgentRegisterDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var agent = await agentService.CreateAgentAsync(dto);
                return Ok(ApiResponse<object>.Ok(
                    new { agent.AgentId, agent.AgentCode, agent.Email },
                    "Agent created. Credentials sent via email."));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResponse<string>.Fail(ex.Message));
            }
        }

        /// [Agent] Create a Customer.
        /// AgentId is auto-resolved from the logged-in agent's JWT — not passed in the body.
        [HttpPost("create-customer")]
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerRegisterDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var authId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var agent  = await db.Agents.FirstOrDefaultAsync(a => a.AuthId == authId);
            if (agent == null) return Forbid();

            try
            {
                var customer = await customerService.CreateByAgentAsync(dto, agent.AgentId);
                return Ok(ApiResponse<object>.Ok(
                    new { customer.CustomerId, customer.CustomerCode, customer.Email, customer.AgentId },
                    "Customer created. Credentials sent via email."));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResponse<string>.Fail(ex.Message));
            }
        }
    }
}
