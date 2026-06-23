using System.ComponentModel.DataAnnotations;


namespace MusicAPI.DTOs.Users
{
    public class UserUpdateDto
    {
        [Required]
        public string FullName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
        // public string Password { get; set; } = string.Empty;
        [Phone]
        public string? PhoneNumber { get; set; } = string.Empty;
        public DateOnly? DateOfBirth { get; set; }
    }
}