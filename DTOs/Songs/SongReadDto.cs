namespace MyApi.DTOs.Songs
{
    public class SongReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public int Duration { get; set; } // Duration in seconds
        public string FileImage { get; set; } = string.Empty;
        public string FileSong { get; set; } = string.Empty;
    }
}