using MusicAPI.Models;

namespace MusicAPI.Interfaces
{
    public interface IArtistRepository : IRepository<Song>
    {
        // Task<bool> DeleteSongAsync(long id);
    }
}