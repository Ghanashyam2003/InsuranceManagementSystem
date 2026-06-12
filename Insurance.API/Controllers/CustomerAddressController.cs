using Insurance.Application.DTOs;
using Insurance.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Asp.Versioning;
using System.Security.Claims;

namespace Insurance.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/customers/{customerId:int}/addresses")]
    [Authorize(Roles = "Customer,Admin,Agent")]
    [EnableRateLimiting("StandardPolicy")]
    public class CustomerAddressController : ControllerBase
    {
        private readonly ICustomerAddressService addressService;
        public CustomerAddressController(ICustomerAddressService addressService) => this.addressService = addressService;
        private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        [HttpPost]
        public async Task<IActionResult> AddAddress(int customerId, [FromBody] AddressDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(GetValidationErrors());
            try { return Ok(new { Message = "Address added successfully", Data = await addressService.AddAddressAsync(customerId, dto, GetUserId()) }); }
            catch (KeyNotFoundException ex) { return NotFound(new { Status = "Error", Message = ex.Message }); }
        }

        [HttpGet]
        public async Task<IActionResult> GetAddresses(int customerId) => Ok(new { Message = "Addresses fetched successfully.", Data = await addressService.GetAddressesByCustomerIdAsync(customerId) });

        [HttpPut("{addressId:int}")]
        public async Task<IActionResult> UpdateAddress(int customerId, int addressId, [FromBody] AddressDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(GetValidationErrors());
            try { return Ok(new { Message = "Address updated successfully", Data = await addressService.UpdateAddressAsync(addressId, dto, GetUserId()) }); }
            catch (KeyNotFoundException ex) { return NotFound(new { Status = "Error", Message = ex.Message }); }
        }

        [HttpDelete("{addressId:int}")]
        public async Task<IActionResult> DeleteAddress(int customerId, int addressId)
        {
            try { await addressService.DeleteAddressAsync(addressId); return Ok(new { Message = "Address deleted successfully." }); }
            catch (KeyNotFoundException ex) { return NotFound(new { Status = "Error", Message = ex.Message }); }
        }

        private object GetValidationErrors() => new { Status = "Error", Message = "Validation failed. Please check your inputs.", Errors = ModelState.Where(e => e.Value!.Errors.Count > 0).ToDictionary(kvp => kvp.Key, kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).FirstOrDefault()) };
    }
}