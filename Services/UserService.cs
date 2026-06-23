using MusicAPI.DTOs.Songs;
using MusicAPI.Models;
using MusicAPI.Interfaces;
using MusicAPI.DTOs.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using AutoMapper;

namespace MusicAPI.Services
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
            var users = await _unitOfWork.Users.GetAllAsync();
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
            await _unitOfWork.Users.AddAsync(newUser);

            // Lưu vào database
            await _unitOfWork.CompleteAsync();

            // Chỉ trả về đối tượng User thuần túy
            return newUser;
        }
        public async Task<bool> UpdateUserAsync(long id, UserUpdateDto userDto)
        {
            // 1. Lấy user hiện có từ DB
            var existingUser = await _unitOfWork.Users.GetByIdAsync(id);
            if (existingUser == null) return false;

            // 2. Kiểm tra nghiệp vụ: Email hoặc Phone có bị trùng không?
            if (userDto.Email != existingUser.Email && await _unitOfWork.Users.IsEmailExistsAsync(userDto.Email))
            {
                throw new ArgumentException("Email đã tồn tại!");
            }
            if (userDto.PhoneNumber != null)
            {
                if (userDto.PhoneNumber != existingUser.PhoneNumber && await _unitOfWork.Users.IsPhoneNumberExistsAsync(userDto.PhoneNumber))
                {
                    throw new ArgumentException("Số điện thoạt đã tồn tại!");
                }
            }


            if (userDto.Role != "Admin" && userDto.Role != "User")
            {
                throw new ArgumentException("Role không hợp lệ");
            }

            // 3. Map dữ liệu từ DTO sang Entity (chỉ cập nhật những trường cho phép)
            existingUser.FullName = userDto.FullName;
            existingUser.Email = userDto.Email;
            existingUser.PhoneNumber = userDto.PhoneNumber;
            existingUser.DateOfBirth = userDto.DateOfBirth;
            existingUser.PhoneNumber = userDto.PhoneNumber;

            // 4. Lưu thay đổi qua Repository
            _unitOfWork.Users.Update(existingUser);
            await _unitOfWork.CompleteAsync();

            return true;
        }
        public async Task DeleteUserAsync(long id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("user not exist!");
            _unitOfWork.Users.Delete(user);
            await _unitOfWork.CompleteAsync();
        }
    }
}
