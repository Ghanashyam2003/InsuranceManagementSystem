using AutoMapper;
using Insurance.Application.Common.Responses;
using Insurance.Application.DTOs.Product;
using Insurance.Application.Interfaces;
using Insurance.Domain.Models;
using Insurance.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Insurance.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;
        private readonly IMemoryCache cache;
        private readonly ILogger<ProductRepository> logger;

        public ProductRepository(ApplicationDbContext db,IMapper mapper,IMemoryCache cache,ILogger<ProductRepository> logger)
        {
            this.db = db;
            this.mapper = mapper;
            this.cache = cache;
            this.logger = logger;
        }






        public async Task<ApiResponse<ProductResponseDto>>AddProduct(ProductCreateDto dto)
        {
            var product = mapper.Map<InsuranceProduct>(dto);

            product.CreatedAt = DateTime.Now;
            product.CreatedBy = 1;

            await db.InsuranceProducts.AddAsync(product);


            await db.SaveChangesAsync();

            cache.Remove("ProductList");

            logger.LogInformation(
                "Product Added Successfully");

            var response = mapper.Map<ProductResponseDto>(product);

            return ResponseHelper.Success(response,"Product Added Successfully");
        }





        public async Task<ApiResponse<List<ProductResponseDto>>>GetAllProducts(int page, int pageSize)
        {
            string cacheKey = $"ProductList_{page}_{pageSize}";

            if (!cache.TryGetValue(cacheKey,out List<ProductResponseDto>? products))
            {
                var data = await db.InsuranceProducts.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                products =mapper.Map<List<ProductResponseDto>>(data);

                var options =
                    new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(
                        TimeSpan.FromMinutes(5));

                cache.Set(cacheKey, products, options);
            }

            logger.LogInformation(
                "Fetched Products Successfully");

            int totalRecords =
                await db.InsuranceProducts.CountAsync();

            return ResponseHelper.Success(products,"Products Fetched Successfully",totalRecords);
        }






        public async Task<ApiResponse<ProductResponseDto>>GetProductById(int id)
        {
            string cacheKey = $"Product_{id}";

            if (!cache.TryGetValue(
                cacheKey,
                out ProductResponseDto? productDto))
            {
                var product = await db.InsuranceProducts.FirstOrDefaultAsync(x => x.ProductId == id);

                if (product == null)
                {
                    return new ApiResponse<ProductResponseDto>
                    {
                        Success = false,
                        Message = "Product Not Found"
                    };
                }

                productDto =mapper.Map<ProductResponseDto>(product);

                cache.Set(cacheKey,productDto,TimeSpan.FromMinutes(5));
            }

            logger.LogInformation(
                "Fetched Product By Id");

            return ResponseHelper.Success(
                productDto,
                "Product Found");
        }





        public async Task<ApiResponse<ProductResponseDto>>UpdateProduct(int id,ProductCreateDto dto)
        {
            var product =
                await db.InsuranceProducts
                .FirstOrDefaultAsync(
                    x => x.ProductId == id);

            if (product == null)
            {
                return new ApiResponse<ProductResponseDto>
                {
                    Success = false,
                    Message = "Product Not Found"
                };
            }

            product.ProductName = dto.ProductName;
            product.ProductType = dto.ProductType;


            product.Description = dto.Description;
            product.Status = dto.Status;

            product.ModifiedAt = DateTime.Now;
            product.ModifiedBy = 1;

            await db.SaveChangesAsync();

            cache.Remove("ProductList");
            cache.Remove($"Product_{id}");

            logger.LogInformation("Product Updated Successfully");

            var response =mapper.Map<ProductResponseDto>(product);

            return ResponseHelper.Success(
                response,
                "Product Updated Successfully");
        }




    }
}