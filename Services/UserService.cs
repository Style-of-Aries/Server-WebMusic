using MyApi.DTOs.Songs;
using MyApi.Models;
using MyApi.Interfaces;
using MyApi.DTOs.Users;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MyApi.Services
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SongService> _logger;
        public UserService(IUnitOfWork unitOfWork, ILogger<SongService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.Users.GetAllUserAsync();
            return users.Select(u => new UserReadDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Role = u.Role
                // Không đưa PasswordHash vào đây!
            });
        }
        public async Task<User> AddUserAsync(UserCreateDto dto)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var isEmailTaken = await _unitOfWork.Users.IsEmailExistsAsync(dto.Email);

            // 2. Kiểm tra giá trị boolean
            if (isEmailTaken)
            {
                throw new InvalidOperationException("Email already exists");
            }
            var newUser = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Role = dto.Role,
                PasswordHash = passwordHash
            };
            // Gọi repository để thêm
            await _unitOfWork.Users.AddUserAsync(newUser);

            // Lưu vào database
            await _unitOfWork.CompleteAsync();

            // Chỉ trả về đối tượng User thuần túy
            return newUser;
        }
    }
}
