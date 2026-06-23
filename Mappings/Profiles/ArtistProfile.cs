using AutoMapper;
using MusicAPI.Models;
// using MusicAPI.DTOs.Users;
// using MusicAPI.DTOs.;
using MusicAPI.DTOs;

public class ArtistProfile : Profile
{
    public ArtistProfile()
    {
        // Map từ Entity User -> UserReadDto
        CreateMap<Artist, ArtistReadDto>();
        // Map từ UserRegisterDto -> Entity User
        CreateMap<ArtistCreateDto, Artist>();
    }
}