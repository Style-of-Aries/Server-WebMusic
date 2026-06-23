using AutoMapper;
using BCrypt.Net;
using MusicAPI.DTOs.Auth;
using MusicAPI.DTOs.Users;
using MusicAPI.Interfaces;
using MusicAPI.Models;

namespace MusicAPI.Services;

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
        var user = await _unitOfWork.Users.GetUserByEmailAsync(dto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Email hoặc mật khẩu không đúng.");

        return _tokenService.CreateToken(user);
    }

    public async Task<string> RegisterAsync(RegisterDto dto)
    {
        // 1. Kiểm tra logic (không nên dùng map ở đây)
        var existingUser = await _unitOfWork.Users.IsEmailExistsAsync(dto.Email);
        if (existingUser) throw new InvalidOperationException("Email đã tồn tại.");

        // 2. Dùng AutoMapper để khởi tạo đối tượng User cơ bản
        var user = _mapper.Map<User>(dto);

        // 3. Xử lý phần mật khẩu (đặc thù nên làm thủ công)
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        // 4. Lưu vào DB
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.CompleteAsync();
        return _tokenService.CreateToken(user);
        // Console.WriteLine("token: ",token);

        // Trả về cả token và thông tin user để Frontend lưu lại
    }
}