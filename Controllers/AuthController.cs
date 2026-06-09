using Microsoft.AspNetCore.Mvc;
using MyApi.DTOs.Auth;
using MyApi.Interfaces;
using MyApi.Services;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            // Middleware sẽ tự bắt các lỗi như UnauthorizedAccessException 
            // và trả về response phù hợp (ví dụ 401)
            var token = await _authService.LoginAsync(dto);
            return Ok(new
            {
                message = "Login successfully",
                token
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            // Middleware sẽ tự bắt các lỗi như InvalidOperationException
            // và trả về response phù hợp (ví dụ 400)
            var token = await _authService.RegisterAsync(dto);
            return Ok(new
            {
                message = "Đăng ký thành công",
                token = token // Trả về trực tiếp chuỗi, không bọc thêm object nữa
            });
        }
    }
}