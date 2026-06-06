using MyApi.Models;

namespace MyApi.Interfaces
{
    public interface ISongRepository
    {
        Task<IEnumerable<Song>> GetAllSongsAsync();
        Task<Song?> GetSongByIdAsync(int id);
        Task<Song> AddSongAsync(Song song);
        Task<bool> UpdateSongAsync(Song song);
        Task<bool> DeleteSongAsync(int id);
    }
}