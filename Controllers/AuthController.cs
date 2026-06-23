using Microsoft.AspNetCore.Mvc;
using MusicAPI.DTOs.Auth;
using MusicAPI.Interfaces;
using MusicAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;


namespace MusicAPI.Controllers
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
                message = "Register successfully",
                token = token // Trả về trực tiếp chuỗi, không bọc thêm object nữa
            });
        }
        [HttpGet("test-token")]
        public IActionResult TestToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // Lấy iat
                var iat = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Iat)?.Value;

                // Chuyển đổi Unix timestamp sang DateTime để dễ đọc
                if (long.TryParse(iat, out long unixTime))
                {
                    var dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime.ToLocalTime();
                    Console.WriteLine($"Token generation time (Local): {dateTime}");
                }

                return Ok(new { IssuedAt = iat });
            }
            catch (Exception ex)
            {
                return BadRequest("Token Invalid: " + ex.Message);
            }
        }
    }
}