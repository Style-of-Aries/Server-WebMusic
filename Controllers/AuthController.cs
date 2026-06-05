using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using MyApi.DTOs.Auth;
using MyApi.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using Microsoft.IdentityModel.Tokens;

using System.Text;

namespace MyApi.Controllers
{
    [ApiController]

    [Route("api/auth")]

    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        public AuthController(ILogger<AuthController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        private static readonly List<User> users = new()
        {
            new User
            {
                Id = 1,
                FullName = "Admin User",
                Email = "admin@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = "Admin"
            }
        };

        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),

                new Claim(ClaimTypes.Email,user.Email),

                new Claim(ClaimTypes.Role,user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    issuer:
                        _configuration["Jwt:Issuer"],

                    audience:
                        _configuration["Jwt:Audience"],

                    claims:
                        claims,

                    expires:
                        DateTime.UtcNow.AddDays(7),

                    signingCredentials:
                        credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {

            // check duplicate email

            var existingUser =
                users.FirstOrDefault(x => x.Email.ToLower() == dto.Email.ToLower());
            if (existingUser != null)
            {
                return BadRequest("Email already exists");
            }
            // hash password
            var passwordHash =
                BCrypt.Net.BCrypt.HashPassword(dto.Password);
            // create user
            var newUser = new User
            {
                Id = users.Count + 1,
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = passwordHash,
                Role = "User"
            };

            users.Add(newUser);

            return Ok(new
            {
                message =
                        "Register success",
                data =
                        new
                        {
                            newUser.Id,
                            newUser.FullName,
                            newUser.Email,
                            newUser.Role
                        }
            }
            );
        }
        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = users.FirstOrDefault(x => x.Email.ToLower() == dto.Email.ToLower());

            if (user == null)
            {
                return Unauthorized(
                    "Invalid email or password"
                );
            }

            var verifyPassword = BCrypt.Net.BCrypt.Verify(
                    dto.Password,
                    user.PasswordHash
                );

            if (!verifyPassword)
            {
                return Unauthorized(
                    "Invalid email or password"
                );
            }
            var token = GenerateToken(user);
            return Ok(
                new
                {
                    message = "Login success",

                    token,

                    user = new
                    {
                        user.Id,

                        user.Email,

                        user.Role
                    }
                }

            );
        }
        [Authorize]

        [HttpGet("me")]

        public async Task<IActionResult> Me()
        {
            var userIdClaim =
                User.FindFirst(
                    ClaimTypes.NameIdentifier
                )?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var user = users.FirstOrDefault(x => x.Id == int.Parse(userIdClaim));

            if (user == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                id = user.Id,
                fullName = user.FullName,
                email = user.Email
            });

        }

    }
}