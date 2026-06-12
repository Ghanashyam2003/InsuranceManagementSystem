using Insurance.Application.DTOs;

namespace Insurance.Application.Interface
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginDto dto);
    }
}