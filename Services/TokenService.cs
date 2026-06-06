using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MyApi.Interfaces;
using MyApi.Models;

public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _key;

    // Lấy secret key từ appsettings.json
    public TokenService(IConfiguration config)
    {
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"] ?? throw new InvalidOperationException("JWT Key is missing in appsettings.json")));
    }

    public string CreateToken(User user)
    {
        // 1. Định nghĩa các Claims (Thông tin nhúng vào token)
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role) // Dùng để kiểm tra quyền Admin/User
        };

        // 2. Tạo chứng chỉ để ký token
        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        // 3. Cấu trúc Token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7), // Token hết hạn sau 7 ngày
            SigningCredentials = creds
        };

        // 4. Tạo token handler
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}