using Insurance.Application.DTO.ClaimInvestigation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimInvestigationController : ControllerBase
    {
        private readonly IClaimInvestigationRepo repo;

        public ClaimInvestigationController( IClaimInvestigationRepo repo)
        {
            this.repo = repo;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await repo.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await repo.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Add(ClaimInvestigationCreateDTO dto)
        {
            return Ok(await repo.Add(dto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update( int id, ClaimInvestigationCreateDTO dto)
        {
            return Ok(await repo.Update(id, dto));
        }
    }
    }
