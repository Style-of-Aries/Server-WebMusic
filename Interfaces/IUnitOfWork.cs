namespace MyApi.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        ISongRepository Songs { get; }
        Task<bool> CompleteAsync();

    }
}