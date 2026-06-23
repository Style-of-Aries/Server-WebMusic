using Microsoft.AspNetCore.Mvc;

namespace MusicAPI.Controllers
{
    [ApiController]
    [Route("api/weather")]
    public class WeatherController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello Controller API");
        }
    }
}