using CodeSphere.Application.Models.User;
using CodeSphere.Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] TelegramAuth auth)
        {
            var token = "8386613782:AAFTsR7Sqn0QVL2r25RgihiJa6l7WJgb48w";
            var result = await _userService.LoginRegister(auth, token);
            {
            }
        }
    }
}
