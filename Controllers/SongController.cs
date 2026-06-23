using Microsoft.AspNetCore.Mvc;
using MusicAPI.Models;
using MusicAPI.Services;
using Microsoft.AspNetCore.Authorization;

using MusicAPI.DTOs.Songs;

namespace MusicAPI.Controllers
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
        public async Task<IActionResult> UpdateSong(long id, [FromForm] SongUpdateDto dto)
        {
            var song = await _songService.UpdateSongAsync(id, dto);
            return Ok(new
            {
                message = "Song updated successfully",
                data = song
            });
        }

        // [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSong(long id)
        {
            await _songService.DeleteSongAsync(id);
            return Ok(new
            {
                message = "Song deleted successfully"
            });
        }

    }
}