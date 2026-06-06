using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using MyApi.DTOs.Users;
using MyApi.Services;

namespace MyApi.Controllers
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
        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var result = _userService.GetAllUsersAsync();
            return Ok(new
            {
                message = "Get all user successfully",
                data = result
            });
        }
        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody]UserCreateDto dto)
        {
            var newUser = await _userService.AddUserAsync(dto);
            return Ok(new
            {
                message = "Add new user successfully"
                // data = newUser
            });
        }
    }
}