using MyApi.DTOs.Songs;
using MyApi.Models;
using MyApi.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MyApi.Services
{
    public class SongService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private readonly ILogger<SongService> _logger;

        public SongService(IUnitOfWork unitOfWork, ILogger<SongService> logger, IMapper mapper, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _env = env;
        }

        public async Task<IEnumerable<Song>> GetAllSongsAsync()
        {
            var songs = await _unitOfWork.Songs.GetAllSongsAsync();
            return songs;
        }
        public async Task<Song> CreateSongAsync(SongCreateDto dto)
        {
            var fileSongName = Guid.NewGuid() + Path.GetExtension(dto.FileSong.FileName);
            var fileImageName = Guid.NewGuid() + Path.GetExtension(dto.FileImage.FileName);
            var musicFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "music");
            var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            Directory.CreateDirectory(musicFolder);
            Directory.CreateDirectory(imageFolder);
            var songPath = Path.Combine(musicFolder, fileSongName);
            var imagePath = Path.Combine(imageFolder, fileImageName);
            using (var stream = new FileStream(songPath, FileMode.Create))
            {
                await dto.FileSong.CopyToAsync(stream);
            }
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await dto.FileImage.CopyToAsync(stream);
            }
            var audio = TagLib.File.Create(songPath);
            // --- 2. Dùng AutoMapper để khởi tạo đối tượng ---
            var newSong = _mapper.Map<Song>(dto);

            // --- 3. Áp dụng logic metadata ---
            newSong.Title = !string.IsNullOrEmpty(dto.Title) ? dto.Title : audio.Tag.Title ?? "Unknown Title";
            newSong.Artist = !string.IsNullOrEmpty(dto.Artist) ? dto.Artist : (audio.Tag.Performers.Length > 0 ? audio.Tag.Performers[0] : "Unknown Artist");
            newSong.Duration = dto.Duration.HasValue && dto.Duration.Value > 0 ? dto.Duration.Value : (int)audio.Properties.Duration.TotalSeconds;

            // Gán đường dẫn file
            newSong.FileSong = $"/music/{fileSongName}";
            newSong.FileImage = $"/images/{fileImageName}";

            // --- 4. Lưu vào DB ---
            await _unitOfWork.Songs.AddSongAsync(newSong);
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation("Song created: {Title} by {Artist}", newSong.Title, newSong.Artist);

            return newSong;
        }
        // SongService.cs
        public async Task<Song> UpdateSongAsync(int id, SongUpdateDto dto)
        {
            // 1. Lấy thông tin bài hát hiện tại từ DB
            var existingSong = await _unitOfWork.Songs.GetSongByIdAsync(id);
            if (existingSong == null) throw new KeyNotFoundException("Không tìm thấy bài hát.");

            // 2. Xử lý Update File Nhạc (nếu có file mới)
            if (dto.FileSong != null)
            {
                // Xóa file cũ
                DeleteFileFromWwwRoot(existingSong.FileSong);

                // Upload file mới
                var fileSongName = Guid.NewGuid() + Path.GetExtension(dto.FileSong.FileName);
                var songPath = Path.Combine(_env.WebRootPath, "music", fileSongName);
                using (var stream = new FileStream(songPath, FileMode.Create))
                {
                    await dto.FileSong.CopyToAsync(stream);
                }
                existingSong.FileSong = $"/music/{fileSongName}";
            }

            // 3. Xử lý Update File Ảnh (nếu có file mới)
            if (dto.FileImage != null)
            {
                // Xóa ảnh cũ
                DeleteFileFromWwwRoot(existingSong.FileImage);

                // Upload ảnh mới
                var fileImageName = Guid.NewGuid() + Path.GetExtension(dto.FileImage.FileName);
                var imagePath = Path.Combine(_env.WebRootPath, "images", fileImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await dto.FileImage.CopyToAsync(stream);
                }
                existingSong.FileImage = $"/images/{fileImageName}";
            }

            // 4. Cập nhật thông tin text (Dùng AutoMapper để map các field còn lại)
            // Đổ dữ liệu từ dto VÀO existingSong
            _mapper.Map(dto, existingSong);
            _unitOfWork.Songs.UpdateSong(existingSong);
            // 5. Lưu thay đổi
            await _unitOfWork.CompleteAsync();

            return existingSong;
        }

        private void DeleteFileFromWwwRoot(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return;

            // _env.WebRootPath thường trỏ tới thư mục "wwwroot"
            // Path.Combine sẽ tự động xử lý dấu gạch chéo dựa trên hệ điều hành (Windows dùng \, Linux dùng /)
            var fullPath = Path.Combine(_env.WebRootPath, relativePath.TrimStart('/'));

            if (File.Exists(fullPath))
            {
                try
                {
                    File.Delete(fullPath);
                }
                catch (IOException ex)
                {
                    // Log lỗi nếu file bị khóa hoặc không thể xóa
                    Console.WriteLine($"Không thể xóa file: {ex.Message}");
                }
            }
        }
        public async Task DeleteSongAsync(int id)
        {
            var song = await _unitOfWork.Songs.GetSongByIdAsync(id)
                ?? throw new KeyNotFoundException("Không tìm thấy bài hát");

            // 1. Xóa trong DB trước
            _unitOfWork.Songs.Remove(song);
            await _unitOfWork.CompleteAsync();

            // 2. Xóa file sau khi DB đã thành công
            DeleteFileFromWwwRoot(song.FileSong); // /music/de55c29f...
            DeleteFileFromWwwRoot(song.FileImage); // /images/048fd2f2...
        }
    }


}