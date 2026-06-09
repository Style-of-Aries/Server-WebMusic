using MyApi.Models;
using MyApi.Data;
using Microsoft.EntityFrameworkCore;
using MyApi.Interfaces;
namespace MyApi.Repositories
{
    public class SongRepository : ISongRepository
    {
        // private readonly List<Song> _songs = new List<Song>();
        private readonly AppDbContext _context;
        public SongRepository(AppDbContext context)
        {
            _context = context;
        }
        // Trong Repository
        public void Remove(Song song)
        {
            _context.Songs.Remove(song); // Dùng trực tiếp _context để tránh nhầm lẫn
        }

        public async Task<IEnumerable<Song>> GetAllSongsAsync()
        {
            var results = await _context.Songs.ToListAsync();
            return results;
            // return await _context.Songs.ToListAsync();
        }

        public async Task<Song?> GetSongByIdAsync(int id)
        {
            var result = await _context.Songs.FirstOrDefaultAsync(s => s.Id == id);
            return result;
            // return await _context.Songs.FirstOrDefaultAsync(s => s.Id == id);
        }

        // Thêm: Trả về đối tượng vừa tạo (để lấy Id từ DB)
        public async Task<Song> AddSongAsync(Song song)
        {
            await _context.Songs.AddAsync(song);
            // XÓA DÒNG SAVECHANGESASYNC Ở ĐÂY
            return song;
        }

        public async Task<bool> UpdateSongAsync(Song song)
        {
            var existingSong = await _context.Songs.FindAsync(song.Id);
            if (existingSong == null) return false;

            existingSong.Title = song.Title;
            existingSong.Artist = song.Artist;

            // XÓA DÒNG SAVECHANGESASYNC Ở ĐÂY
            return true;
        }

        // SongRepository.cs
        public void UpdateSong(Song song)
        {
            // Đánh dấu đối tượng là Modified để EF Core tự cập nhật các thay đổi
            _context.Entry(song).State = EntityState.Modified;
        }

        // public async Task<bool> DeleteSongAsync(int id)
        // {
        //     var song = await _context.Songs.FindAsync(id);
        //     if (song == null) return false;

        //     _context.Songs.Remove(song);
        //     // XÓA DÒNG SAVECHANGESASYNC Ở ĐÂY
        //     return true;
        // }
    }
}