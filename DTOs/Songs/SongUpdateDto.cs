namespace MyApi.DTOs.Songs
{
    public class SongUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public int Duration { get; set; } // Duration in seconds
        public IFormFile? FileImage { get; set; } = null!;
        public IFormFile? FileSong { get; set; } = null!;
        
    }
}