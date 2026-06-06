using MyApi.DTOs.Auth;
using MyApi.Interfaces;
// using Microsoft.AspNetCore.Http.HttpResults;

namespace MyApi.Services;
public class AuthService
{
    private readonly IAuthRepository _authRepo;
    private readonly ITokenService _tokenService; // Service riêng để tạo token

    public AuthService(IAuthRepository authRepo, ITokenService tokenService)
    {
        _authRepo = authRepo;
        _tokenService = tokenService;
    }

    public async Task<string> LoginAsync(LoginDto dto)
    {
        // 1. Lấy user từ DB
        var user = await _authRepo.GetUserByEmailAsync(dto.Email);
        if (user == null) throw new ArgumentException("Email hoặc mật khẩu không đúng.");

        // 2. Kiểm tra mật khẩu
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new ArgumentException("Email hoặc mật khẩu không đúng.");

        // 3. Tạo JWT Token
        return _tokenService.CreateToken(user);
    }
}