using MusicAPI.Models;
// using MusicAPI.Models;
using Microsoft.EntityFrameworkCore;
using MusicAPI.Interfaces;
namespace MusicAPI.Repositories
{
    public class ArtistRepository : BaseRepository<Song>, ISongRepository
    {
        // private readonly List<Song> _songs = new List<Song>();
        public ArtistRepository(AppDbContext context) : base(context) { }
        // Trong Repository
    }
}