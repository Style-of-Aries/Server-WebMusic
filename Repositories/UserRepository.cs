using MyApi.Models;
using MyApi.Data;
using Microsoft.EntityFrameworkCore;
using MyApi.Interfaces;
namespace MyApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        // private readonly List<Song> _songs = new List<Song>();
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            var results = await _context.Users.ToListAsync();
            return results;
            // return await _context.Songs.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(long id)
        {
            var result = await _context.Users.FirstOrDefaultAsync(s => s.Id == id);
            return result;
            // return await _context.Songs.FirstOrDefaultAsync(s => s.Id == id);
        }

        // Thêm: Trả về đối tượng vừa tạo (để lấy Id từ DB)
        public async Task<User> AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            // XÓA DÒNG SAVECHANGESASYNC Ở ĐÂY
            return user;
        }

        public async Task<User?> UpdateUserAsync(User user)
        {
            // 1. Tìm bản ghi hiện tại trong DB (Tracking)
            var existingUser = await _context.Users.FindAsync(user.Id);

            if (existingUser == null)
                return null;

            // 2. Cập nhật các trường dữ liệu cần thiết
            existingUser.FullName = user.FullName;
            existingUser.Email = user.Email;
            existingUser.DateOfBirth = user.DateOfBirth;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Role = user.Role; // Cẩn thận với quyền hạn

            // 3. Quan trọng: Cập nhật thời gian sửa đổi
            existingUser.UpdatedAt = DateTime.UtcNow;

            // KHÔNG gọi SaveChangesAsync ở đây (để dành cho Unit of Work)
            return existingUser;
        }

        public async Task<bool> DeleteUserAsync(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            // XÓA DÒNG SAVECHANGESASYNC Ở ĐÂY
            return true;
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            // Bỏ qua nếu email null hoặc rỗng
            if (string.IsNullOrWhiteSpace(email)) return false;

            return await _context.Users.AnyAsync(u => u.Email == email);
        }
        public async Task<bool> IsPhoneNumberExistsAsync(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) return false;

            return await _context.Users.AnyAsync(u => u.PhoneNumber == phoneNumber);
        }
    }
}