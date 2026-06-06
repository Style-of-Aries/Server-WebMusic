using MyApi.Models;

namespace MyApi.Interfaces
{
    public interface IUserRepository
    {
        // Trả về IEnumerable hoặc IQueryable để lấy danh sách
        Task<IEnumerable<User>> GetAllUserAsync();
        
        Task<User?> GetUserByIdAsync(long id);
        
        // Thêm hàm kiểm tra tồn tại (đúng yêu cầu của bạn)
        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> IsPhoneNumberExistsAsync(string phoneNumber);
        
        Task<User> AddUserAsync(User user);
        Task<User?> UpdateUserAsync(User user);
        
        // Chỉ cần truyền id để xóa, giảm bớt việc phải fetch object trước khi xóa
        Task<bool> DeleteUserAsync(long id); 
    }
}