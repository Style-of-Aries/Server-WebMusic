using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MusicAPI.Models;
using MusicAPI.DTOs.Users;
using MusicAPI.Services;

namespace MusicAPI.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }
        // [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var result = await _userService.GetAllUsersAsync();
            return Ok(new
            {
                message = "Get all user successfully",
                data = result
            });
        }
        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody] UserCreateDto dto)
        {
            var newUser = await _userService.AddUserAsync(dto);
            return Ok(new
            {
                message = "Add new user successfully",
                // data = newUser
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync(long id, [FromBody] UserUpdateDto dto)
        {
            await _userService.UpdateUserAsync(id, dto);
            return Ok(new
            {
                message = $"Update user id = {id} successfully"
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(long id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok(new
            {
                message = "User deleted successfully"
            });
        }
    }
}