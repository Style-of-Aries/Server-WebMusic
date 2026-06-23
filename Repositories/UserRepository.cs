using MusicAPI.Models;
// using MusicAPI.Models;
using Microsoft.EntityFrameworkCore;
using MusicAPI.Interfaces;

namespace MusicAPI.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        // Chỉ cần viết logic cho các hàm nghiệp vụ mới, các hàm CRUD đã được kế thừa từ BaseRepository
        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> IsPhoneNumberExistsAsync(string phoneNumber)
        {
            return await _context.Users.AnyAsync(u => u.PhoneNumber == phoneNumber);
        }
    }
}