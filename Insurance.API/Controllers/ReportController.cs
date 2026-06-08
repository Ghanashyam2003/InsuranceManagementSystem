using Insurance.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.API.Controllers
{
    [ApiController]
    [Route("api/v1/reports")]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepo service;

        public ReportController(
            IReportRepo service)
        {
            this.service = service;
        }

        [HttpGet("policies")]public async Task<IActionResult>
            GetPolicyReport()
        {
            var result =
                await service.GetPolicyReportAsync();

            return Ok(result);
        }
        [HttpGet("premium-collection")]
        public async Task<IActionResult>GetPremiumCollectionReport()
        {
            var result =
                await service.GetPremiumCollectionReportAsync();

            return Ok(result);
        }
        [HttpGet("revenue")]
        public async Task<IActionResult>GetRevenueReport()
        {
            var result =
                await service.GetRevenueReportAsync();

            return Ok(result);
        }
        [HttpGet("agent-performance")]
        public async Task<IActionResult>GetAgentPerformanceReport()
        {
            var result =
                await service.GetAgentPerformanceReportAsync();

            return Ok(result);
        }
        [HttpGet("claims")]
        public async Task<IActionResult>GetClaimReport()
        {
            var result =
                await service.GetClaimReportAsync();

            return Ok(result);
        }
    }
}