using MyApi.Models;
using MyApi.Data;
using Microsoft.EntityFrameworkCore;
namespace MyApi.Repositories
{
    public class SongRepository
    {
        // private readonly List<Song> _songs = new List<Song>();
        private readonly AppDbContext _context;
        public SongRepository(AppDbContext context)
        {
            _context = context;
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
            await _context.SaveChangesAsync();
            return song; // Trả về để controller biết bài hát vừa tạo là gì
        }

        // Sửa: Dùng Update thay vì Remove + Add
        public async Task<Song?> UpdateSongAsync(Song song)
        {
            var existingSong = await _context.Songs.FindAsync(song.Id);
            if (existingSong == null) return null;

            // Cập nhật các trường dữ liệu
            existingSong.Title = song.Title;
            existingSong.Artist = song.Artist;
            // ... update các trường khác

            await _context.SaveChangesAsync();
            return existingSong; // Trả về đối tượng đã sửa
        }

        // Xóa: Trả về bool (true nếu xóa thành công, false nếu không tìm thấy)
        public async Task<bool> DeleteSongAsync(int id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song == null) return false;

            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}