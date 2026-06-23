using System.ComponentModel.DataAnnotations;

namespace MusicAPI.DTOs
{
    public class ArtistCreateDto
    {
        // Có thể để string? để dễ dàng kiểm tra xem user có nhập hay không
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public FormFile? AvatarUrl { get; set; }
    }
}