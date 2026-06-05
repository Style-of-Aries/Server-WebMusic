using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using MyApi.Services;
using Microsoft.AspNetCore.Authorization;

using MyApi.DTOs.Songs;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/song")]
    public class SongController : ControllerBase
    {
        private readonly ILogger<SongController> _logger;
        private readonly SongService _songService;
        public SongController(ILogger<SongController> logger, SongService songService)
        {
            _logger = logger;
            _songService = songService;
        }

        private static readonly List<Song> songs = new();
        [HttpGet]
        // Hãy khai báo async cho Controller và dùng await
        public async Task<IActionResult> GetAll()
        {
            var result = await _songService.GetAllSongsAsync();
            return Ok(new
            {
                message = "Get Songs successfully",
                data = result
            });
        }
        // [Authorize(Roles = "Admin")] 
        [HttpPost]
        public async Task<IActionResult> AddSong([FromForm] SongCreateDto dto)
        {
            var newSong = await _songService.CreateSongAsync(dto);
            return Ok(new
            {
                message = "Song added successfully",
                data = newSong
            });
        }

        // [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSong(int id, [FromForm] SongUpdateDto dto)
        {
            var song = songs.FirstOrDefault(s => s.Id == id);
            if (song == null)
                return NotFound();

            if (dto.FileSong != null)
            {
                var fileSongName = Guid.NewGuid() + Path.GetExtension(dto.FileSong.FileName);
                var musicFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "music");
                Directory.CreateDirectory(musicFolder);
                var songPath = Path.Combine(musicFolder, fileSongName);

                using (var stream = new FileStream(songPath, FileMode.Create))
                {
                    await dto.FileSong.CopyToAsync(stream);
                }

                song.FileSong = $"/music/{fileSongName}";
            }

            if (dto.FileImage != null)
            {
                var fileImageName = Guid.NewGuid() + Path.GetExtension(dto.FileImage.FileName);
                var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                Directory.CreateDirectory(imageFolder);
                var imagePath = Path.Combine(imageFolder, fileImageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await dto.FileImage.CopyToAsync(stream);
                }

                song.FileImage = $"/images/{fileImageName}";
            }

            if (!string.IsNullOrEmpty(dto.Title))
                song.Title = dto.Title;

            if (!string.IsNullOrEmpty(dto.Artist))
                song.Artist = dto.Artist;

            if (dto.Duration > 0)
                song.Duration = dto.Duration;

            _logger.LogInformation(
            "Song updated successfully \n Id: {Id} \n Title: {Title} \n Artist: {Artist}",
            song.Id,
            song.Title,
            song.Artist
            );
            return Ok(new
            {
                message = "Song updated successfully",
                data = song
            });
        }

        // [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteSong(int id)
        {
            var song = songs.FirstOrDefault(s => s.Id == id);
            if (song == null)
                return NotFound();


            songs.Remove(song);
            _logger.LogInformation(
            "Song deleted successfully \n Id: {Id} \n Title: {Title} \n Artist: {Artist}",
            song.Id,
            song.Title,
            song.Artist
            );
            return Ok(new
            {
                message = "Song deleted successfully"
            });
        }
    }
}