using AutoMapper;
using MyApi.Models;
using MyApi.DTOs.Songs;

public class SongProfile : Profile
{
    public SongProfile()
    {
        CreateMap<Song, SongReadDto>();
        CreateMap<SongCreateDto, Song>()
            // Các trường Title, Artist, Duration sẽ được xử lý thủ công trong Service
            // vì có logic kiểm tra null/default, nên ta bỏ qua ở đây
            .ForMember(dest => dest.Title, opt => opt.Ignore())
            .ForMember(dest => dest.Artist, opt => opt.Ignore())
            .ForMember(dest => dest.Duration, opt => opt.Ignore())
            // Các trường đường dẫn file thì map trực tiếp
            .ForMember(dest => dest.FileSong, opt => opt.Ignore())
            .ForMember(dest => dest.FileImage, opt => opt.Ignore());
        CreateMap<SongUpdateDto, Song>()

            // Các trường đường dẫn file thì map trực tiếp
            .ForMember(dest => dest.FileSong, opt => opt.Ignore())
            .ForMember(dest => dest.FileImage, opt => opt.Ignore());
    }
}