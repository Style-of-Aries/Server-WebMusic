using MyApi.Models;

namespace MyApi.Interfaces
{
    public interface ISongRepository
    {
        void Remove(Song song);
        Task<IEnumerable<Song>> GetAllSongsAsync();
        Task<Song?> GetSongByIdAsync(int id);
        
        Task<Song> AddSongAsync(Song song);
        Task<bool> UpdateSongAsync(Song song);
        void UpdateSong(Song song);
        // Task<bool> DeleteSongAsync(int id);
    }
}