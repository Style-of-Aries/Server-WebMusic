using MusicAPI.Models;
using Microsoft.EntityFrameworkCore;
using MusicAPI.Interfaces;
// using MusicAPI.Models;

namespace MusicAPI.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly AppDbContext _context;

        public FavoriteRepository(AppDbContext context) => _context = context;

        public async Task<bool> IsFavoriteAsync(long userId, long songId)
            => await _context.Favorites.AnyAsync(f => f.UserId == userId && f.SongId == songId);

        public async Task AddFavoriteAsync(long userId, long songId)
        {
            _context.Favorites.Add(new Favorite { UserId = userId, SongId = songId });
            // await _context.SaveChangesAsync();
        }

        public async Task RemoveFavoriteAsync(long userId, long songId)
        {
            var fav = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.SongId == songId);
            if (fav != null)
            {
                _context.Favorites.Remove(fav);
                // await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<long>> GetUserFavoriteSongIdsAsync(long userId)
            => await _context.Favorites.Where(f => f.UserId == userId).Select(f => f.SongId).ToListAsync();

        public async Task<long> GetFavoriteCountAsync(long songId)
            => await _context.Favorites.CountAsync(f => f.SongId == songId);
    }
}