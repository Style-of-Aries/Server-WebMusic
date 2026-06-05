namespace MyApi.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string FileSong { get; set; } = string.Empty;
        public string FileImage { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public int Duration { get; set; } // Duration in seconds
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}