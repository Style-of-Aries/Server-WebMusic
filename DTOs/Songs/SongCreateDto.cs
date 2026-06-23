using System.ComponentModel.DataAnnotations;

namespace MusicAPI.DTOs.Songs
{
    public class SongCreateDto
    {
        // Có thể để string? để dễ dàng kiểm tra xem user có nhập hay không
        public string? Title { get; set; }
        public string? Artist { get; set; }
        public int? Duration { get; set; }

        // Dùng [Required] attribute để đảm bảo API không bị gọi nếu thiếu file
        [Required]
        public IFormFile FileImage { get; set; } = null!;

        [Required]
        public IFormFile FileSong { get; set; } = null!;
    }
}