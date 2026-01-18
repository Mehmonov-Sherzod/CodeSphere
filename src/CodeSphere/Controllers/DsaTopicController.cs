using CodeSphere.Application.Models;
using CodeSphere.Application.Models.DsaTopic;
using CodeSphere.Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DsaTopicController : ControllerBase
    {
        private readonly IDsaTopicService _dsaTopicService;

        public DsaTopicController(IDsaTopicService dsaTopicService)
        {
            _dsaTopicService = dsaTopicService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateDsaTopic(CreateDsaTopic createDsaTopic)
        {
            var result = await _dsaTopicService.CreateDsaTopic(createDsaTopic);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("get-all-page")]
        public async Task<IActionResult> GetAllDsaTopicsPage(PageOption pageOption)
        {
            var result = await _dsaTopicService.GetAllDsaTopicsPage(pageOption);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDsaTopicById(Guid id)
        {
            var result = await _dsaTopicService.GetDsaTopicById(id);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDsaTopic(UpdateDsaTopic updateDsaTopic, Guid id)
        {
            var result = await _dsaTopicService.UpdateDsaTopic(updateDsaTopic, id);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDsaTopic(Guid id)
        {
            var result = await _dsaTopicService.DeleteDsaTopic(id);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
