// using MusicAPI.Models;

namespace MusicAPI.Interfaces
{
    public interface IFavoriteRepository
    {
        // Kiểm tra xem user đã thích bài hát chưa
        Task<bool> IsFavoriteAsync(long userId, long songId);

        // Thêm vào danh sách yêu thích
        Task AddFavoriteAsync(long userId, long songId);

        // Xóa khỏi danh sách yêu thích
        Task RemoveFavoriteAsync(long userId, long songId);

        // Lấy danh sách ID các bài hát đã thích (Dùng cho trang "Thư viện của tôi")
        Task<IEnumerable<long>> GetUserFavoriteSongIdsAsync(long userId);

        // Lấy số lượng lượt thích của một bài hát (Cần thiết cho hiển thị)
        Task<long> GetFavoriteCountAsync(long songId);
    }
}