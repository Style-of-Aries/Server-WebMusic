namespace MusicAPI.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        // IUserRepositoryNew UsersNew { get; }
        ISongRepository Songs { get; }
        // IAuthRepository Auth { get; }
        IFavoriteRepository Favorite { get; }
        Task<bool> CompleteAsync();

    }
}