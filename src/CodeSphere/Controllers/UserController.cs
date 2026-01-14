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
        [HttpPost("login-register")]
        public async Task<IActionResult> LoginRegister([FromBody] TelegramAuth auth, [FromHeader] string token)
        {
            var result = await _userService.LoginRegister(auth, token);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
