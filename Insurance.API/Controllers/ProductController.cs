using Asp.Versioning;
using Insurance.Application.DTOs.Product;
using Insurance.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Insurance.API.Controllers
{
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/v1/[controller]")]

    [ApiController]
    [ApiVersion("1.0")]
    [EnableRateLimiting("ApiThrottle")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository repository;

        public ProductController(IProductRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductCreateDto dto)
        {
            var result =await repository.AddProduct(dto);

            return Ok(result);
        }




        [HttpGet]
        public async Task<IActionResult> GetAllProducts(int page = 1,int pageSize = 10)
        {
            var result = await repository.GetAllProducts(page,pageSize);

            return Ok(result);
        }




        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result =await repository.GetProductById(id);

            return Ok(result);
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id,ProductCreateDto dto)
        {
            var result =await repository.UpdateProduct(id,dto);

            return Ok(result);
        }
    }
}