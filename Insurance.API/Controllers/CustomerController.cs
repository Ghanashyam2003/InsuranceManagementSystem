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
    // Tells .NET this is an API controller, enabling automatic behavior like binding JSON bodies
    [ApiController]
    [ApiVersion("1.0")] // Defines this controller as API Version 1.0 for future-proofing
    [Route("api/v{version:apiVersion}/[controller]")] // Generates routes like /api/v1/Customer
    [EnableRateLimiting("StandardPolicy")] // Protects this controller with our req/min rule
    public class CustomerController : ControllerBase
    {
        // Holds our business logic service
        private readonly ICustomerService customerService;

        // .NET automatically injects the customerService here
        public CustomerController(ICustomerService customerService) => this.customerService = customerService;

        // POST: /api/v1/Customer
        // Handles both self-registration (public) and agents creating customers internally
        [HttpPost]
        [AllowAnonymous] // Anyone can hit this endpoint to register
        public async Task<IActionResult> Create([FromBody] CustomerRegisterDto dto)
        {
            // Validate the incoming data first (e.g., checking for valid PAN/Aadhaar formats)
            if (!ModelState.IsValid)
                return BadRequest(GetValidationErrors());
            try
            {
                // Check if the person making the request is an already logged-in Agent or Admin
                if (User.Identity?.IsAuthenticated == true && (User.IsInRole("Agent") || User.IsInRole("Admin")))
                {
                    // Extract the agent's ID from their token and link the new customer to them
                    var agentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                    var customer = await customerService.CreateByAgentAsync(dto, agentId);
                    return Ok(new
                    {
                        Message = "Customer created successfully.",
                        Data = new { customer.CustomerId, customer.CustomerCode, customer.Email }
                    });
                }
                else
                {
                    // Otherwise, treat it as a standard self-registration from the public website
                    var customer = await customerService.SelfRegisterAsync(dto);
                    return Ok(new
                    {
                        Message = "Registration successful. Please login.",
                        Data = new { customer.CustomerId, customer.CustomerCode, customer.Email }
                    });
                }
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Status = "Error", Message = ex.Message });
            } // Catches duplicates like "Email already exists"
        }


        // GET: /api/v1/Customer?page=1&pageSize=10
        // Used by Admins to see the full list of customers (with pagination so it doesn't crash the server)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool? isActive = null) // Added the missing filter
        {
            //  cap the page size at 50 to prevent massive database pulls
            if (pageSize > 50) pageSize = 50;
            if (page < 1) page = 1;

            var result = await customerService.GetAllAsync(page, pageSize, isActive);

            // Map domain models to safe objects so we don't accidentally leak passwords or cause JSON loop errors
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

        // GET: /api/v1/Customer/user
        // Allows a logged-in customer to view their own profile safely
        [HttpGet("user")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyProfile()
        {
            var customer = await GetLoggedInCustomerAsync();
            if (customer == null)
                return NotFound(new { Status = "Error", Message = "Profile not found." });
            return Ok(new { Message = "Profile fetched successfully.", Data = MapCustomer(customer) });
        }

        // GET: /api/v1/Customer/{id}
        // Allows Admins or Agents to look up a specific customer by ID
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Agent")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await customerService.GetByIdAsync(id);
            if (customer == null)
                return NotFound(new { Status = "Error", Message = "Customer not found." });
            return Ok(new { Message = "Customer fetched successfully.", Data = MapCustomer(customer) });
        }

        // PUT: /api/v1/Customer/user
        // Allows a customer to update their own details
        [HttpPut("user")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] CustomerProfileUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(GetValidationErrors());
            try
            {
                var customer = await GetLoggedInCustomerAsync();
                if (customer == null)
                    return NotFound(new { Status = "Error", Message = "Customer not found." });
                var updated = await customerService.UpdateProfileAsync(customer.CustomerId, dto, "Customer");
                return Ok(new
                {
                    Message = "Profile updated successfully.",
                    Data = MapCustomer(updated)
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Status = "Error", Message = ex.Message });
            }
        }

        // PUT: /api/v1/Customer/{id}
        // Allows an Admin to update ANY customer's details
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
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Status = "Error", Message = ex.Message });
            }
        }

        // DELETE: /api/v1/Customer/user
        // Customer deletes themselves (Hard delete - completely removes their data from the DB)
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
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Status = "Error", Message = ex.Message });
            }
        }

        // DELETE: /api/v1/Customer/{id}
        // Admin deletes a customer (Soft delete - just marks IsActive as false)
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
                await customerService.DeleteAsync(id, "Admin");
                return Ok(new { Message = "Customer deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Status = "Error", Message = ex.Message });
            }
        }

        // Helper method: Extracts the user ID from the active JWT token and finds the matching customer
        private async Task<Domain.Models.Customer?> GetLoggedInCustomerAsync()
        {
            var authId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return await customerService.GetByAuthIdAsync(authId);
        }

        // Helper method: Flattens .NET's complex validation errors into a simple dictionary for the frontend
        private object GetValidationErrors()
        {
            var errors = ModelState
                .Where(e => e.Value!.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value!.Errors
                .Select(e => e.ErrorMessage).FirstOrDefault());
            return new { Status = "Error", Message = "Validation failed. Please check your inputs.", Errors = errors };
        }

        // Helper method: Strips out sensitive or unnecessary database fields before sending the customer object to the client
        private static object MapCustomer(Domain.Models.Customer c) => new { c.CustomerId, c.CustomerCode, c.Email, c.FirstName, c.LastName, c.MobileNumber, c.DOB, c.Gender, c.PANNumber, c.AadharNumber, c.AgentId, c.IsDeleted, c.CreatedAt, c.ModifiedAt };
    }
}