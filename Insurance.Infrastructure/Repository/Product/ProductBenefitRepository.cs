using AutoMapper;
using Insurance.Application.Common.Responses;
using Insurance.Application.DTOs.ProductBenefit;
using Insurance.Application.Interfaces;
using Insurance.Domain.Models;
using Insurance.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Insurance.Infrastructure.Repositories
{
    public class ProductBenefitRepository : IProductBenefitRepository
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;
        private readonly IMemoryCache cache;
        private readonly ILogger<ProductBenefitRepository> logger;

        public ProductBenefitRepository(
            ApplicationDbContext db,
            IMapper mapper,IMemoryCache cache,ILogger<ProductBenefitRepository> logger)
        {
            this.db = db;
            this.mapper = mapper;
            this.cache = cache;
            this.logger = logger;
        }

        public async Task<ApiResponse<ProductBenefitResponseDto>>AddBenefit(ProductBenefitCreateDto dto)
        {
            var benefit = mapper.Map<ProductBenefit>(dto);

            benefit.CreatedAt = DateTime.Now;

            benefit.CreatedBy = 1;

            await db.ProductBenefits.AddAsync(benefit);

            await db.SaveChangesAsync();

            cache.Remove("BenefitList");

            logger.LogInformation("Benefit Added Successfully");

            var response =
                mapper.Map<ProductBenefitResponseDto>(benefit);

            return ResponseHelper.Success(
                response,
                "Benefit Added Successfully");
        }

        public async Task<ApiResponse<List<ProductBenefitResponseDto>>>GetAllBenefits()
        {
            string cacheKey = "BenefitList";

            if (!cache.TryGetValue(
                cacheKey,
                out List<ProductBenefitResponseDto>? benefits))
            {
                var data = await db.ProductBenefits.ToListAsync();

                benefits =mapper.Map<List<ProductBenefitResponseDto>>(data);

                cache.Set(
                    cacheKey,
                    benefits,
                    TimeSpan.FromMinutes(5));
            }

            logger.LogInformation(
                "Benefits Fetched Successfully");

            return ResponseHelper.Success(
                benefits,
                "Benefits Fetched Successfully");
        }

        public async Task<ApiResponse<ProductBenefitResponseDto>>GetBenefitById(int id)
        {
            var benefit = await db.ProductBenefits.FirstOrDefaultAsync(x => x.BenefitId == id);

            if (benefit == null)
            {
                return new ApiResponse<ProductBenefitResponseDto>
                {
                    Success = false,
                    Message = "Benefit Not Found"
                };
            }

            var response = mapper.Map<ProductBenefitResponseDto>(benefit);

            return ResponseHelper.Success(
                response,
                "Benefit Found");
        }

        public async Task<ApiResponse<List<ProductBenefitResponseDto>>>GetBenefitsByProductId(int productId)
        {
            string cacheKey = $"Benefit_Product_{productId}";

            if (!cache.TryGetValue(
                cacheKey,
                out List<ProductBenefitResponseDto>? benefits))
            {
                var data = await db.ProductBenefits.Where(x => x.ProductId == productId).ToListAsync();

                benefits =mapper.Map<List<ProductBenefitResponseDto>>(data);

                cache.Set(
                    cacheKey,
                    benefits,
                    TimeSpan.FromMinutes(5));
            }

            logger.LogInformation(
                "Benefits Fetched By Product");

            return ResponseHelper.Success(
                benefits,
                "Benefits Fetched Successfully");
        }

        public async Task<ApiResponse<ProductBenefitResponseDto>>UpdateBenefit(int id,ProductBenefitCreateDto dto)
        {
            var benefit =
                await db.ProductBenefits
                .FirstOrDefaultAsync(
                    x => x.BenefitId == id);

            if (benefit == null)
            {
                return new ApiResponse<ProductBenefitResponseDto>
                {
                    Success = false,
                    Message = "Benefit Not Found"
                };
            }

            benefit.ProductId = dto.ProductId;
            benefit.BenefitName = dto.BenefitName;
            benefit.BaseRate = dto.BaseRate;
            benefit.Minimum = dto.Minimum;
            benefit.Maximum = dto.Maximum;

            benefit.ModifiedAt = DateTime.Now;
            benefit.ModifiedBy = 1;

            await db.SaveChangesAsync();

            cache.Remove("BenefitList");
            cache.Remove($"Benefit_Product_{dto.ProductId}");

            logger.LogInformation(
                "Benefit Updated Successfully");

            var response =mapper.Map<ProductBenefitResponseDto>(benefit);

            return ResponseHelper.Success(
                response,
                "Benefit Updated Successfully");
        }
    }
}