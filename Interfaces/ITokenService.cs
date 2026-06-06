using MyApi.Models; // Namespace chứa class User của bạn

namespace MyApi.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}