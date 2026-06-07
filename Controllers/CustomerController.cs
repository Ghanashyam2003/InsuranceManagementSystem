using Insurance.Application.DTOs;
using Insurance.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Asp.Versioning;
using System.Security.Claims;
using System.Linq;

namespace Insurance.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")] // API VERSIONING
    [Route("api/v{version:apiVersion}/[controller]")]
    [EnableRateLimiting("StandardPolicy")] // RATE LIMITING / THROTTLING
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;
        public CustomerController(ICustomerService customerService) => this.customerService = customerService;

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CustomerRegisterDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(GetValidationErrors());
            try
            {
                if (User.Identity?.IsAuthenticated == true && (User.IsInRole("Agent") || User.IsInRole("Admin")))
                {
                    var agentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                    var customer = await customerService.CreateByAgentAsync(dto, agentId);
                    return Ok(new { Message = "Customer created by agent successfully.", Data = new { customer.CustomerId, customer.CustomerCode, customer.Email } });
                }
                else
                {
                    var customer = await customerService.SelfRegisterAsync(dto);
                    return Ok(new { Message = "Registration successful. Please login.", Data = new { customer.CustomerId, customer.CustomerCode, customer.Email } });
                }
            }
            catch (InvalidOperationException ex) { return Conflict(new { Status = "Error", Message = ex.Message }); }
        }

        // ─────────────────────────────────────────────
        // FIXED: GET ALL 
        // 1. Added isActive filter 
        // 2. Mapped the list to prevent JSON cyclic errors
        // ─────────────────────────────────────────────
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool? isActive = null) // Added the missing filter
        {
            if (pageSize > 50) pageSize = 50;
            if (page < 1) page = 1;

            var result = await customerService.GetAllAsync(page, pageSize, isActive);

            // Map domain models to safe objects
            var safeItems = result.Items.Select(c => MapCustomer(c)).ToList();

            var pagedResponse = new
            {
                Items = safeItems,
                result.TotalCount,
                result.Page,
                result.PageSize,
                result.TotalPages
            };

            return Ok(new { Message = "Customers fetched successfully.", Data = pagedResponse });
        }

        [HttpGet("user")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyProfile()
        {
            var customer = await GetLoggedInCustomerAsync();
            if (customer == null) return NotFound(new { Status = "Error", Message = "Profile not found." });
            return Ok(new { Message = "Profile fetched successfully.", Data = MapCustomer(customer) });
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Agent")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await customerService.GetByIdAsync(id);
            if (customer == null) return NotFound(new { Status = "Error", Message = "Customer not found." });
            return Ok(new { Message = "Customer fetched successfully.", Data = MapCustomer(customer) });
        }

        [HttpPut("user")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] CustomerProfileUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(GetValidationErrors());
            try
            {
                var customer = await GetLoggedInCustomerAsync();
                if (customer == null) return NotFound(new { Status = "Error", Message = "Customer not found." });
                var updated = await customerService.UpdateProfileAsync(customer.CustomerId, dto, "Customer");
                return Ok(new { Message = "Profile updated successfully.", Data = MapCustomer(updated) });
            }
            catch (KeyNotFoundException ex) { return NotFound(new { Status = "Error", Message = ex.Message }); }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateById(int id, [FromBody] CustomerProfileUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(GetValidationErrors());
            try
            {
                var updated = await customerService.UpdateProfileAsync(id, dto, "Admin");
                return Ok(new { Message = "Customer updated successfully.", Data = MapCustomer(updated) });
            }
            catch (KeyNotFoundException ex) { return NotFound(new { Status = "Error", Message = ex.Message }); }
        }

        [HttpDelete("user")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> DeleteMyAccount()
        {
            try
            {
                var customer = await GetLoggedInCustomerAsync();
                if (customer == null) return NotFound(new { Status = "Error", Message = "Customer not found." });
                await customerService.HardDeleteAsync(customer.CustomerId);
                return Ok(new { Message = "Your account and all data have been permanently deleted." });
            }
            catch (KeyNotFoundException ex) { return NotFound(new { Status = "Error", Message = ex.Message }); }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteById(int id)
        {
            try { await customerService.DeleteAsync(id, "Admin"); return Ok(new { Message = "Customer deleted successfully." }); }
            catch (KeyNotFoundException ex) { return NotFound(new { Status = "Error", Message = ex.Message }); }
        }

        private async Task<Domain.Models.Customer?> GetLoggedInCustomerAsync()
        {
            var authId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return await customerService.GetByAuthIdAsync(authId);
        }

        private object GetValidationErrors()
        {
            var errors = ModelState.Where(e => e.Value!.Errors.Count > 0).ToDictionary(kvp => kvp.Key, kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).FirstOrDefault());
            return new { Status = "Error", Message = "Validation failed. Please check your inputs.", Errors = errors };
        }

        private static object MapCustomer(Domain.Models.Customer c) => new { c.CustomerId, c.CustomerCode, c.Email, c.FirstName, c.LastName, c.MobileNumber, c.DateOfBirth, c.Gender, c.PanNumber, c.AadhaarNumber, c.AgentId, c.IsActive, c.CreatedAt, c.ModifiedAt };
    }
}