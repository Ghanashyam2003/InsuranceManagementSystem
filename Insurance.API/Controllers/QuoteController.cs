using Insurance.Application.DTOs.Quote;
using Insurance.Application.Interfaces;
using Insurance.Application.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteController : ControllerBase
    {
        private readonly IQuoteRepo repo;

        public QuoteController(
            IQuoteRepo repo)
        {
            this.repo = repo;
        }

        [HttpPost("generate")]
        public async Task<IActionResult>
            GenerateQuote(
            CreateQuoteDto dto)
        {
            var result =
                await repo
                    .GenerateQuote(dto);

            return Ok(
                ResponseHelper.Success(
                    result,
                    "Quote Generated Successfully"));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllQuotes()
        {
            var result = await repo.GetAllQuotes();

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