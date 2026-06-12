using Insurance.Application.DTO.Claim;
using Insurance.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimSettlementController : ControllerBase
    {
        IClaimSettlementRepo repo;

        public ClaimSettlementController(IClaimSettlementRepo repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await repo.GetAll();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await repo.GetById(id);

            if (data == null)
                return NotFound("Settlement Not Found");

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Add(
            ClaimSettlementCreateDTO dto)
        {
            var result = await repo.Add(dto);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            ClaimSettlementCreateDTO dto)
        {
            var result = await repo.Update(id, dto);

            return Ok(result);
        }
    }
}

