using Insurance.Application.DTOs;
using Insurance.Application.Interface;
using Insurance.Application.Interfaces;
using Insurance.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext db;
        private readonly IJwtService jwt;

        public AuthService(ApplicationDbContext db, IJwtService jwt)
        {
            this.db = db;
            this.jwt = jwt;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto dto)
        {
            var auth = await db.Auths
                .Include(a => a.Role)
                .FirstOrDefaultAsync(a => a.Email == dto.Email);

            if (auth == null) return null;

            // Plain-text password comparison
            if (auth.Password != dto.Password) return null;

            return new LoginResponseDto
            {
                Token = jwt.GenerateToken(auth),
                Role = auth.Role.RoleName,
                Email = auth.Email
            };
        }
    }
}