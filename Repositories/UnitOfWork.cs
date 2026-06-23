using MusicAPI.Interfaces;
using MusicAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MusicAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IUserRepository Users { get; private set; }
        public ISongRepository Songs { get; private set; }
        // public IAuthRepository Auth { get; private set; }
        public IFavoriteRepository Favorite { get; private set; }

        // Hàm Constructor nạp cả repository thông qua Dependency Injection (DI)
        public UnitOfWork(
            AppDbContext context,
            IUserRepository userRepository,
            // IUserRepositoryNew userRepositoryNew,
            // IAuthRepository authRepository,
            IFavoriteRepository favoriteRepository,
            ISongRepository songRepository)
        {
            _context = context;
            Songs = songRepository;
            Users = userRepository;
            // UsersNew = userRepositoryNew;
            Favorite = favoriteRepository;
            // Auth = authRepository;
        }

        public async Task<bool> CompleteAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}