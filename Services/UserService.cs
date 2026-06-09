using MyApi.DTOs.Songs;
using MyApi.Models;
using MyApi.Interfaces;
using MyApi.DTOs.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using AutoMapper;

namespace MyApi.Services
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<SongService> _logger;
        public UserService(IUnitOfWork unitOfWork, ILogger<SongService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.Users.GetAllUserAsync();
            // AutoMapper sẽ tự hiểu: Nếu đầu vào là IEnumerable, 
            // đầu ra sẽ là IEnumerable<UserReadDto>
            return _mapper.Map<IEnumerable<UserReadDto>>(users);
        }
        public async Task<User> AddUserAsync(UserCreateDto dto)
        {
            // 1. Guard Clause: Kiểm tra điều kiện đầu vào
            if (await _unitOfWork.Users.IsEmailExistsAsync(dto.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }
            // var newUser = new User
            // {
            //     FullName = dto.FullName,
            //     Email = dto.Email,
            //     Role = dto.Role,
            //     PasswordHash = passwordHash
            // };
            var newUser = _mapper.Map<User>(dto);
            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            // Nếu bạn muốn Role mặc định là 'User', bạn có thể set ở đây hoặc trong Profile
            if (string.IsNullOrEmpty(newUser.Role)) newUser.Role = "User";

            // Gọi repository để thêm
            await _unitOfWork.Users.AddUserAsync(newUser);

            // Lưu vào database
            await _unitOfWork.CompleteAsync();

            // Chỉ trả về đối tượng User thuần túy
            return newUser;
        }
        public async Task DeleteUserAsync(long id)
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(id)
                ?? throw new KeyNotFoundException("user not exist!");
            _unitOfWork.Users.Remove(user);
            await _unitOfWork.CompleteAsync();
        }
    }
}
