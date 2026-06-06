using Microsoft.EntityFrameworkCore;
using MyApi.Data;
using MyApi.Interfaces;
using MyApi.Models;

namespace MyApi.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            // Tìm user theo email (cần include Role nếu bạn quản lý Role ở bảng riêng)
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }
    }
}   