// using MusicAPI.DTOs.Songs;
// using MusicAPI.Models;
// using MusicAPI.Interfaces;
// using MusicAPI.DTOs.Users;
// using Microsoft.AspNetCore.Http.HttpResults;
// using System.Security.Claims;
// using AutoMapper;

// namespace MusicAPI.Services
// {
//     public class FavoriteService
//     {
//         private readonly IUnitOfWork _unitOfWork;
//         private readonly IMapper _mapper;
//         public FavoriteService(IUnitOfWork unitOfWork, IMapper mapper)
//         {
//             _unitOfWork = unitOfWork;
//             _mapper = mapper;
//         }
//         public Task<bool> ToggleFavorite(long songId)
//         {
//             // Sử dụng toán tử null-conditional ?. để lấy giá trị một cách an toàn
//             var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//             // Kiểm tra xem userIdClaim có null hay rỗng không
//             if (string.IsNullOrEmpty(userIdClaim))
//             {
//                 return Unauthorized("Không tìm thấy thông tin người dùng trong token.");
//             }

//             // Bây giờ bạn có thể an toàn khi thực hiện parse
//             var userId = long.Parse(userIdClaim);
//         }
//     }
// }