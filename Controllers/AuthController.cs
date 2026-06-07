using Insurance.Application.DTOs;
using Insurance.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Asp.Versioning;

namespace Insurance.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")] //Defines this controller as API Version 1.0
    [Route("api/v{version:apiVersion}/[controller]")] // Route will be /api/v1/Auth
    [EnableRateLimiting("StandardPolicy")] 
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService) => this.authService = authService;

        /// Login for Admin / Agent / Customer 
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            // 4. Manually handles validation to return the clean dictionary format
            if (!ModelState.IsValid)
                return BadRequest(GetValidationErrors());

            var result = await authService.LoginAsync(dto);
            if (result == null)
                return Unauthorized(ApiResponse<string>.Fail("Invalid email or password."));

            return Ok(ApiResponse<LoginResponseDto>.Ok(result, "Login successful."));
        }

       
        private object GetValidationErrors()
        {
            var errors = ModelState.Where(e => e.Value!.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
                );
            return new { Status = "Error", Message = "Validation failed. Please check your inputs.", Errors = errors };
        }
    }
}