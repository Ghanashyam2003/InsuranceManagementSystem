using Asp.Versioning;
using Insurance.Application.DTOs.ProductBenefit;
using Insurance.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Insurance.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [EnableRateLimiting("ApiThrottle")]
    public class ProductBenefitController : ControllerBase
    {
        private readonly IProductBenefitRepository repository;

        public ProductBenefitController(IProductBenefitRepository repository)
        {
            this.repository = repository;
        }



        [HttpPost]
        public async Task<IActionResult> AddBenefit(
            ProductBenefitCreateDto dto)
        {
            var result = await repository.AddBenefit(dto);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBenefits()
        {
            var result =  await repository.GetAllBenefits();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBenefitById(int id)
        {
            var result = await repository.GetBenefitById(id);


            return Ok(result);
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult>GetBenefitsByProductId(int productId)
        {
            var result = await repository.GetBenefitsByProductId(productId);

            return Ok(result);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBenefit(int id,ProductBenefitCreateDto dto)
        {
            var result =await repository.UpdateBenefit(id,dto);

            return Ok(result);
        }

    }
}