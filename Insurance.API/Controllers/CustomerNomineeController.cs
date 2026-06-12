using Insurance.Application.DTOs;
using Insurance.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Asp.Versioning;
using System.Security.Claims;
using Insurance.Application.Interface;

namespace Insurance.Api.Controllers
{
    // Marks this as an API controller, enabling automatic model binding and validation behaviors
    [ApiController]
    // Sets the API version to 1.0 to help manage future breaking changes smoothly
    [ApiVersion("1.0")]
    // Creates a dynamic route that includes the API version (e.g., /api/v1/customers/5/nominees)
    [Route("api/v{version:apiVersion}/customers/{customerId:int}/nominees")]
    // Restricts access to this endpoint to only authenticated Customers, Admins, and Agents
    [Authorize(Roles = "Customer,Admin,Agent")]
    // Defends against spam/DDoS attacks by applying our global 100 requests/minute rule
    [EnableRateLimiting("StandardPolicy")]
    public class CustomerNomineeController : ControllerBase
    {
        // Holds our business logic service to interact with the database
        private readonly ICustomerNomineeService nomineeService;

        // Constructor: .NET automatically provides (injects) the nominee service when the controller starts
        public CustomerNomineeController(ICustomerNomineeService nomineeService) => this.nomineeService = nomineeService;

        // Helper method: Extracts the logged-in user's unique ID directly from their secure JWT token
        private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        // POST endpoint: Adds a new nominee to a specific customer's policy
        [HttpPost]
        public async Task<IActionResult> AddNominee(int customerId, [FromBody] NomineeDto dto)
        {
            // Verify the incoming data (like ensuring percentage is between 0.01 and 100) before proceeding
            if (!ModelState.IsValid) return BadRequest(GetValidationErrors());
            try { return Ok(new { Message = "Nominee added successfully", Data = await nomineeService.AddNomineeAsync(customerId, dto, GetUserId()) }); }
            // Catches business rule violations, like if the total nominee share exceeds 100%
            catch (Exception ex) when (ex is KeyNotFoundException || ex is InvalidOperationException) { return BadRequest(new { Status = "Error", Message = ex.Message }); }
        }

        // GET endpoint: Retrieves the list of all nominees attached to a customer
        [HttpGet]
        public async Task<IActionResult> GetNominees(int customerId) => Ok(new { Message = "Nominees fetched successfully.", Data = await nomineeService.GetNomineesByCustomerIdAsync(customerId) });

        // PUT endpoint: Updates an existing nominee's details or share percentage
        [HttpPut("{nomineeId:int}")]
        public async Task<IActionResult> UpdateNominee(int customerId, int nomineeId, [FromBody] NomineeDto dto)
        {
            // Check for valid data formats (e.g., no numbers in the nominee name)
            if (!ModelState.IsValid) return BadRequest(GetValidationErrors());
            try { return Ok(new { Message = "Nominee updated successfully", Data = await nomineeService.UpdateNomineeAsync(nomineeId, dto, GetUserId()) }); }
            // Ensures updates don't break business rules (like pushing the customer's total share over 100%)
            catch (Exception ex) when (ex is KeyNotFoundException || ex is InvalidOperationException) { return BadRequest(new { Status = "Error", Message = ex.Message }); }
        }

        // DELETE endpoint: Completely removes a nominee from the customer's account
        [HttpDelete("{nomineeId:int}")]
        public async Task<IActionResult> DeleteNominee(int customerId, int nomineeId)
        {
            try { await nomineeService.DeleteNomineeAsync(nomineeId); return Ok(new { Message = "Nominee deleted successfully." }); }
            // Returns a 404 Not Found if the nominee was already deleted or doesn't exist
            catch (KeyNotFoundException ex) { return NotFound(new { Status = "Error", Message = ex.Message }); }
        }

        // Helper method: Takes messy .NET validation errors and converts them into a clean, easy-to-read JSON object for the frontend team
        private object GetValidationErrors() => new { Status = "Error", Message = "Validation failed. Please check your inputs.", Errors = ModelState.Where(e => e.Value!.Errors.Count > 0).ToDictionary(kvp => kvp.Key, kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).FirstOrDefault()) };
    }
}