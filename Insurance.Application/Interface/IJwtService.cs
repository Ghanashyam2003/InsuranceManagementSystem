using Insurance.Domain.Models;

namespace Insurance.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(Auth auth);
    }
}