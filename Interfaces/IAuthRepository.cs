using MyApi.Models;

namespace MyApi.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> UserExistsAsync(string email);
        // Có thể thêm các hàm khác nếu cần
    }
}