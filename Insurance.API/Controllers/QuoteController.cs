using Asp.Versioning;
using Insurance.Application.Common.Responses;
using Insurance.Application.DTOs.Quote;
using Insurance.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Insurance.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [EnableRateLimiting("ApiThrottle")]
    //[Route("api/[controller]")]
    [ApiController]
    public class QuoteController : ControllerBase
    {
        private readonly IQuoteRepo repo;
        private readonly ILogger<QuoteController> logger;

        public QuoteController(
    IQuoteRepo repo,
    ILogger<QuoteController> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }

        [HttpPost("generate")]
        public async Task<IActionResult>
            GenerateQuote(
            CreateQuoteDto dto)
        {
            var result =
                await repo
                    .GenerateQuote(dto);

            logger.LogInformation(
    "Generating quote for CustomerId {CustomerId}",
    dto.CustomerId);

            return Ok(
                ResponseHelper.Success(
                    result,
                    "Quote Generated Successfully"));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllQuotes(
    int pageNumber = 1,
    int pageSize = 10)
        {
            var result =
                await repo.GetAllQuotes(
                    pageNumber,
                    pageSize);

            logger.LogInformation(
                "Fetching quotes PageNumber {PageNumber} PageSize {PageSize}",
                pageNumber,
                pageSize);

            return Ok(
                ResponseHelper.Success(
                    result,
                    "Quotes fetched successfully"
                )
            );
        }


        [HttpGet("{id}/premium-breakdown")]
        public async Task<IActionResult> GetPremiumBreakdown(int id)
        {
            var result = await repo.GetPremiumBreakdown(id);

            logger.LogInformation(
    "Fetching premium breakdown for QuoteId {QuoteId}",
    id);

            return Ok(
                ResponseHelper.Success(
                    result,
                    "Premium breakdown fetched successfully"
                )
            );
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetQuotesByCustomerId(int customerId)
        {
            var result = await repo.GetQuotesByCustomerId(customerId);

            return Ok(
                ResponseHelper.Success(
                    result,
                    "Customer quotes fetched successfully"
                )
            );
        }

        [HttpPost("{id}/accept")]
        public async Task<IActionResult> AcceptQuote(int id)
        {
            var result = await repo.AcceptQuote(id);

            logger.LogInformation(
    "Accepting Quote {QuoteId}",
    id);

            return Ok(
                ResponseHelper.Success(
                    result,
                    "Quote accepted successfully"
                )
            );
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> RejectQuote(int id)
        {
            var result = await repo.RejectQuote(id);

            logger.LogInformation(
    "Rejecting Quote {QuoteId}",
    id);

            return Ok(
                ResponseHelper.Success(
                    result,
                    "Quote rejected successfully"
                )
            );
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingQuotes()
        {
            var result = await repo.GetQuotesByStatus("Pending");

            return Ok(
                ResponseHelper.Success(
                    result,
                    "Pending quotes fetched successfully"
                )
            );
        }

        [HttpGet("accepted")]
        public async Task<IActionResult> GetAcceptedQuotes()
        {
            var result = await repo.GetQuotesByStatus("Accepted");

            return Ok(
                ResponseHelper.Success(
                    result,
                    "Accepted quotes fetched successfully"
                )
            );
        }


        [HttpGet("rejected")]
        public async Task<IActionResult> GetRejectedQuotes()
        {
            var result = await repo.GetQuotesByStatus("Rejected");

            return Ok(
                ResponseHelper.Success(
                    result,
                    "Rejected quotes fetched successfully"
                )
            );
        }
    }
}