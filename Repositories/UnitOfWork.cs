using MyApi.Interfaces;
using MyApi.Data;
using Microsoft.EntityFrameworkCore;

namespace MyApi.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IUserRepository Users { get; private set; }  
        public ISongRepository Songs { get; private set; }

        // Hàm Constructor nạp cả repository thông qua Dependency Injection (DI)
        public UnitOfWork(
            AppDbContext context,
            IUserRepository userRepository,
            ISongRepository songRepository)
        {
            _context = context;
            Songs = songRepository;
            Users = userRepository;
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