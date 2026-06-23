// using System.Text.Json.Serialization;

namespace MusicAPI.DTOs.Users
{
    public class UserReadDto
    {
        public long Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        // [JsonConverter(typeof(DateOnlyConverter))]
        public DateOnly? DateOfBirth { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}