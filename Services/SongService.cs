using MyApi.DTOs.Songs;
using MyApi.Models;
using MyApi.Interfaces;

namespace MyApi.Services
{
    public class SongService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SongService> _logger;

        public SongService(IUnitOfWork unitOfWork, ILogger<SongService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<Song>> GetAllSongsAsync()
        {
            var songs = await _unitOfWork.Songs.GetAllSongsAsync();
            return songs;
        }
        public async Task<Song> CreateSongAsync(SongCreateDto dto)
        {
            var fileSongName = Guid.NewGuid() + Path.GetExtension(dto.FileSong.FileName);
            var fileImageName = Guid.NewGuid() + Path.GetExtension(dto.FileImage.FileName);
            var musicFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "music");
            var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            Directory.CreateDirectory(musicFolder);
            Directory.CreateDirectory(imageFolder);
            var songPath = Path.Combine(musicFolder, fileSongName);
            var imagePath = Path.Combine(imageFolder, fileImageName);
            using (var stream = new FileStream(songPath, FileMode.Create))
            {
                await dto.FileSong.CopyToAsync(stream);
            }
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await dto.FileImage.CopyToAsync(stream);
            }
            var audio = TagLib.File.Create(songPath);
            var newSong = new Song
            {
                Title = !string.IsNullOrEmpty(dto.Title)
                    ? dto.Title
                    : audio.Tag.Title ?? "Unknown Title",

                Artist = !string.IsNullOrEmpty(dto.Artist)
                    ? dto.Artist
                    : (audio.Tag.Performers.Length > 0
                        ? audio.Tag.Performers[0]
                        : "Unknown Artist"),

                Duration =
    dto.Duration.HasValue && dto.Duration.Value > 0
        ? dto.Duration.Value
        : (int)audio.Properties.Duration.TotalSeconds,
                FileSong = $"/music/{fileSongName}",
                FileImage = $"/images/{fileImageName}"
            };

            await _unitOfWork.Songs.AddSongAsync(newSong);
            _logger.LogInformation("Song created: {Title} by {Artist}", newSong.Title, newSong.Artist);
            return newSong;
        }
    }


}