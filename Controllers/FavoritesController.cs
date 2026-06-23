using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAPI.Models;
// using MusicAPI.Models;
using System.Security.Claims;

namespace MusicAPI.Controllers
{
    [Authorize] // Yêu cầu đăng nhập
    [Route("api/favorite")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FavoritesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("toggle/{songId}")]
        public async Task<IActionResult> ToggleFavorite(long songId)
        {
            // Sử dụng toán tử null-conditional ?. để lấy giá trị một cách an toàn
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Kiểm tra xem userIdClaim có null hay rỗng không
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("Không tìm thấy thông tin người dùng trong token.");
            }

            // Bây giờ bạn có thể an toàn khi thực hiện parse
            var userId = long.Parse(userIdClaim);
            // 2. Kiểm tra xem bài hát có tồn tại không (Rất quan trọng để tránh lỗi khóa ngoại)
            var songExists = await _context.Songs.AnyAsync(s => s.Id == songId);
            if (!songExists) return NotFound("Bài hát không tồn tại.");

            // Tìm xem đã có bản ghi này chưa
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.SongId == songId);

            if (favorite != null)
            {
                // XÓA HẲN: Loại bỏ khỏi bảng Favorites
                _context.Favorites.Remove(favorite);
            }
            else
            {
                // THÊM MỚI
                _context.Favorites.Add(new Favorite { UserId = userId, SongId = songId });
            }

            // Lưu thay đổi vào DB
            await _context.SaveChangesAsync();

            // Trả về trạng thái sau cùng để Client cập nhật UI
            return Ok(new { IsLiked = favorite == null });
        }
    }
}