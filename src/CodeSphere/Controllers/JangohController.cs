using CodeSphere.Application.Models.JangohModels;
using CodeSphere.Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JangohController : ControllerBase
    {
        private readonly IJangohService _jangohService;
        public JangohController(IJangohService jangohService)
        {
            _jangohService = jangohService;
        }
        [HttpPost("submit-code")]
        public async Task<IActionResult> SubmitCode(SubmitCodes codes)
        {
            var result = await _jangohService.SubmitCode(codes);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
