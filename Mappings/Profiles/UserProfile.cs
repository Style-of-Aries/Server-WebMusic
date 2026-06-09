using AutoMapper;
using MyApi.Models;
using MyApi.DTOs.Users;
using MyApi.DTOs.Auth;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // Map từ Entity User -> UserReadDto
        CreateMap<User, UserReadDto>();
        // Map từ UserRegisterDto -> Entity User
        CreateMap<RegisterDto, User>()
    .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Bỏ qua password vì ta sẽ hash thủ công
    .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "User")); // Mặc định role là User
        CreateMap<UserCreateDto, User>()
        .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        
    }
}