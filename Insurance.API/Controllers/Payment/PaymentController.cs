using Asp.Versioning;
using Insurance.Application.Common.Responses;
using Insurance.Application.DTO.Payment;
using Insurance.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Insurance.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [EnableRateLimiting("ApiThrottle")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository repository;

        public PaymentController(
            IPaymentRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult>
            MakePayment(MakePaymentDto dto)
        {
            var result =
                await repository.MakePaymentAsync(dto);

            if (!result)
            {
                return BadRequest(
                    ResponseHelper.Fail(
                        "Payment Failed",
                        "PAYMENT_ERROR",
                        "Schedule Not Found"));
            }

            return Ok(
                ResponseHelper.Success(
                    true,
                    "Payment Successful"));
        }

        [HttpGet]
        public async Task<IActionResult>
            GetAllPayments()
        {
            var result =
                await repository.GetAllPaymentsAsync();

            return Ok(
                ResponseHelper.Success(
                    result,
                    "Payments Fetched Successfully",
                    result.Count));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult>
            GetPaymentById(int id)
        {
            var result =
                await repository.GetPaymentByIdAsync(id);

            if (result == null)
            {
                return NotFound(
                    ResponseHelper.Fail(
                        "Payment Not Found",
                        "NOT_FOUND",
                        "Invalid Payment Id"));
            }

            return Ok(
                ResponseHelper.Success(
                    result,
                    "Payment Found"));
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult>
            GetPaymentHistory(int customerId)
        {
            var result =
                await repository
                    .GetPaymentHistoryByCustomerAsync(
                        customerId);

            return Ok(
                ResponseHelper.Success(
                    result,
                    "Payment History Fetched",
                    result.Count));
        }

        [HttpGet("success")]
        public async Task<IActionResult>
            GetSuccessPayments()
        {
            var result =
                await repository
                    .GetSuccessfulPaymentsAsync();

            return Ok(
                ResponseHelper.Success(
                    result,
                    "Success Payments Fetched",
                    result.Count));
        }

        [HttpGet("failed")]
        public async Task<IActionResult>
            GetFailedPayments()
        {
            var result =
                await repository
                    .GetFailedPaymentsAsync();

            return Ok(
                ResponseHelper.Success(
                    result,
                    "Failed Payments Fetched",
                    result.Count));
        }

        [HttpGet("pending")]
        public async Task<IActionResult>
            GetPendingPayments()
        {
            var result =
                await repository
                    .GetPendingPaymentsAsync();

            return Ok(
                ResponseHelper.Success(
                    result,
                    "Pending Payments Fetched",
                    result.Count));
        }

        [HttpGet("due/{customerId}")]
        public async Task<IActionResult>
            GetDuePremiums(int customerId)
        {
            var result =
                await repository
                    .GetDuePremiumsAsync(customerId);

            return Ok(
                ResponseHelper.Success(
                    result,
                    "Due Premiums Fetched",
                    result.Count));
        }
    }
}