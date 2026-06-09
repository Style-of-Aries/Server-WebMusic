namespace MyApi.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        ISongRepository Songs { get; }
        IAuthRepository Auth { get; }
        Task<bool> CompleteAsync();

    }
}