// using Microsoft.EntityFrameworkCore;
// using MusicAPI.Models;

// namespace MusicAPI.Data
// {
//     public class AppDbContext : DbContext
//     {
//         public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

//         public DbSet<User> Users { get; set; }
//         public DbSet<Song> Songs { get; set; }

//         // Thêm DbSet cho Favorite
//         public DbSet<Favorite> Favorites { get; set; }

//         protected override void OnModelCreating(ModelBuilder modelBuilder)
//         {
//             base.OnModelCreating(modelBuilder);

//             // Thiết lập Khóa chính kép (Composite Key) cho bảng Favorite
//             modelBuilder.Entity<Favorite>()
//                 .HasKey(f => new { f.UserId, f.SongId });

//             // Cấu hình quan hệ (Relationship) để đảm bảo toàn vẹn dữ liệu
//             modelBuilder.Entity<Favorite>()
//                 .HasOne(f => f.User)
//                 .WithMany(u => u.Favorites)
//                 .HasForeignKey(f => f.UserId)
//                 .OnDelete(DeleteBehavior.Cascade); // Xóa User thì xóa luôn các lượt thích của họ

//             modelBuilder.Entity<Favorite>()
//                 .HasOne(f => f.Song)
//                 .WithMany(s => s.FavoritedBy)
//                 .HasForeignKey(f => f.SongId)
//                 .OnDelete(DeleteBehavior.Cascade); // Xóa Song thì xóa luôn các lượt thích liên quan
//         }
//     }
// }