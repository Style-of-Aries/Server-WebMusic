namespace MusicAPI.Interfaces
{

    // Ví dụ bằng C# / .NET (hoặc áp dụng tương tự cho PHP)
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(long id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}