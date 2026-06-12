using Insurance.Application.DTO.Claim;
using Insurance.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        private readonly IClaimRepo repo;

        public ClaimController(IClaimRepo Repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetClaims()
        {
            var data = await repo.GetClaims();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await repo.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Add(ClaimDTO2 dto)
        {
            return Ok(await repo.Add(dto));
        }

        

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,ClaimDTO2 dto)
        {
            return Ok(await repo.Update(id, dto));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await repo.Delete(id));
        }
        [HttpGet("policy/{policyId}")]
        public async Task<IActionResult> GetByPolicyId(int policyId)
        {
            var data = await repo.GetByPolicyId(policyId);

            if (data == null || !data.Any())
                return NotFound("No Claims Found For This Policy");

            return Ok(data);
        }

    }
}
