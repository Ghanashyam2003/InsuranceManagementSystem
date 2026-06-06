using Insurance.Application.Common.Responses;
using Insurance.Application.DTOs.ProductBenefit;

namespace Insurance.Application.Interfaces
{
    public interface IProductBenefitRepository
    {
        Task<ApiResponse<ProductBenefitResponseDto>>
            AddBenefit(ProductBenefitCreateDto dto);

        Task<ApiResponse<List<ProductBenefitResponseDto>>>
            GetAllBenefits();

        Task<ApiResponse<ProductBenefitResponseDto>>
            GetBenefitById(int id);

        Task<ApiResponse<List<ProductBenefitResponseDto>>>
            GetBenefitsByProductId(int productId);

        Task<ApiResponse<ProductBenefitResponseDto>>
            UpdateBenefit(int id,
            ProductBenefitCreateDto dto);
    }
}