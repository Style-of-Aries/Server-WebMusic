using AutoMapper;
using BCrypt.Net;
using MyApi.DTOs.Auth;
using MyApi.DTOs.Users;
using MyApi.Interfaces;
using MyApi.Models;

namespace MyApi.Services;

public class AuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly TokenService _tokenService;

    public AuthService(IMapper mapper, IUnitOfWork unitOfWork, TokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    public async Task<string> LoginAsync(LoginDto dto)
    {
        var user = await _unitOfWork.Auth.GetUserByEmailAsync(dto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Email hoặc mật khẩu không đúng.");

        return _tokenService.CreateToken(user);
    }

    public async Task<string> RegisterAsync(RegisterDto dto)
    {
        // 1. Kiểm tra logic (không nên dùng map ở đây)
        var existingUser = await _unitOfWork.Auth.GetUserByEmailAsync(dto.Email);
        if (existingUser != null) throw new InvalidOperationException("Email đã tồn tại.");

        // 2. Dùng AutoMapper để khởi tạo đối tượng User cơ bản
        var user = _mapper.Map<User>(dto);

        // 3. Xử lý phần mật khẩu (đặc thù nên làm thủ công)
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        // 4. Lưu vào DB
        await _unitOfWork.Users.AddUserAsync(user);
        await _unitOfWork.CompleteAsync();
        return _tokenService.CreateToken(user);
        // Console.WriteLine("token: ",token);

        // Trả về cả token và thông tin user để Frontend lưu lại
    }
    public async Task<UserReadDto> GetUserByIdAsync(int userId)
    {
        var user = await _unitOfWork.Users.GetUserByIdAsync(userId); // Hoặc logic bạn đang dùng
        if (user == null) throw new KeyNotFoundException("User không tồn tại");

        return _mapper.Map<UserReadDto>(user);
    }
}