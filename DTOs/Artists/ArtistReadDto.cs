using System.ComponentModel.DataAnnotations;

namespace MusicAPI.DTOs
{
    public class ArtistReadDto
    {
        // Có thể để string? để dễ dàng kiểm tra xem user có nhập hay không
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Bio { get; set; }
        public FormFile? AvatarUrl { get; set; }
    }
}