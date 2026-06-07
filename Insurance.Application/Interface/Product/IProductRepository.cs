using Insurance.Application.Common.Responses;
using Insurance.Application.DTOs.Product;

namespace Insurance.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<ApiResponse<ProductResponseDto>> AddProduct(ProductCreateDto dto);

        Task<ApiResponse<List<ProductResponseDto>>> GetAllProducts(
            int page,
            int pageSize);
        Task<ApiResponse<ProductResponseDto>> GetProductById(int id);

        Task<ApiResponse<ProductResponseDto>> UpdateProduct(int id,
            ProductCreateDto dto);
    }
}