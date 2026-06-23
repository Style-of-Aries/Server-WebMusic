using MusicAPI.Models;

namespace MusicAPI.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByEmailAsync(string email);
        // Trả về IEnumerable hoặc IQueryable để lấy danh sách
        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> IsPhoneNumberExistsAsync(string phoneNumber);
    }
}