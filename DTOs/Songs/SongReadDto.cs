namespace MusicAPI.DTOs.Songs
{
    public class SongReadDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Performer { get; set; } = string.Empty;
        public int Duration { get; set; } // Duration in seconds
        public string FileImage { get; set; } = string.Empty;
        public string FileSong { get; set; } = string.Empty;
    }
}